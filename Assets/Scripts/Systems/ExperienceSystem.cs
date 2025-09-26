using System;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour, IObservable
{
    public static ExperienceSystem Instance;

    [Header("Experience")]
    [SerializeField] private float _maxExperienceLimit = 9999f;
    [SerializeField] private float _startExperienceThreshold = 10f;
    [SerializeField] private float _experienceGainMultiplier = 1.2f;
    private float _currentExperience = 0f;
    private float _currentExperienceThreshold = 0f;

    [Header("Level")]
    [SerializeField] private int _maxLevel = 100;
    private int _startLevel = 1;
    private int _currentLevel = 0;

    [Header("Perks")]
    [SerializeField] private int _maxPerksCount = 9999; 
    [SerializeField] private int _currentPerksCount;
    public int CurrentPerksCount => _currentPerksCount;

    private List<IObserver> _observers = new List<IObserver>();

    //// Events for UI
    //public event Action<int, int> OnXpChanged;

    //// Properties for UI
    //public int CurrentXP => Mathf.RoundToInt(_currentExperience);
    //public int XpToNext => Mathf.RoundToInt(_currentExperienceThreshold);

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _currentExperienceThreshold = _startExperienceThreshold;
        _currentLevel = _startLevel;
    }

    private void Start()
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentExperience, _currentExperienceThreshold);
            obs.UpdateData(_currentLevel);
        }

        //// Trigger UI event
        //OnXpChanged?.Invoke(CurrentXP, XpToNext);
    }

    public void AddExperience(float value)
    {
        if (value <= 0f) return;

        _currentExperience += value;

        _currentExperience = Mathf.Clamp(_currentExperience, 0f, _maxExperienceLimit);

        while (_currentExperience >= _currentExperienceThreshold)
        {
            _currentExperience -= _currentExperienceThreshold;
            AddLevel();
            //_observers[0].UpdateData(_currentLevel);
            RecalculateExperienceThreshold();
        }

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentExperience, _currentExperienceThreshold);
            obs.UpdateData(_currentLevel);
        }

        //// Trigger UI event
        //OnXpChanged?.Invoke(CurrentXP, XpToNext);
    }

    private void AddLevel()
    {
        if (_currentLevel < _maxLevel)
        {
            _currentLevel++;
        }
    }

    public void AddPerks()
    {
        _currentPerksCount++;
    }

    public void SubstractPerk(int value)
    {
        if (value <= 0f) return;

        _currentPerksCount -= value;

        _currentPerksCount = Mathf.Clamp(_currentPerksCount, 0, _maxPerksCount);
    }

    private void RecalculateExperienceThreshold()
    {
        _currentExperienceThreshold *= _experienceGainMultiplier;
    }

    public void Subscribe(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    #region TEST
    [ContextMenu("Add Experience")]
    public void AddExperienceForce()
    {
        AddExperience(100);
    }
    #endregion
}