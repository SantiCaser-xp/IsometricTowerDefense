using System.Collections.Generic;
using Unity.Services.RemoteConfig;
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
        RemoteConfigService.Instance.FetchCompleted += UpdateData;
        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentExperience, _currentExperienceThreshold);
            obs.UpdateData(_currentLevel);
        }
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
            RecalculateExperienceThreshold();
        }

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentExperience, _currentExperienceThreshold);
            obs.UpdateData(_currentLevel);
        }
    }

    private void AddLevel()
    {
        if (_currentLevel < _maxLevel)
        {
            _currentLevel++;
            AddPerk();
        }
    }

    public void AddPerk(int value = 1)
    {
        _currentPerksCount = Mathf.Clamp(_currentPerksCount + value, 0, _maxPerksCount);
    }

    public bool TryUsePerk(int cost)
    {
        if (_currentPerksCount >= cost)
        {
            _currentPerksCount = Mathf.Clamp(_currentPerksCount - cost, 0, _maxPerksCount);
            return true;
        }
        return false;
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

    public void UpdateData(ConfigResponse configResponse)
    {
        var experienceToAdd = RemoteConfigService.Instance.appConfig.GetFloat("XpToAdd");
        AddExperience(experienceToAdd);

    }

    #region TEST
    [ContextMenu("Add Experience")]
    public void AddExperienceForce()
    {
        AddExperience(100);
    }
    #endregion
}