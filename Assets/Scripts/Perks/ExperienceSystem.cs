using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperienceSystem : SingltonBase<ExperienceSystem>, IObservable
{
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

    protected override void Awake()
    {
        base.Awake();

        _currentExperienceThreshold = _startExperienceThreshold;
        _currentLevel = _startLevel;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
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
        EventManager.Trigger(EventType.OnPerkChanged);
    }

    public void SubstractPerk(int cost)
    {
        if (_currentPerksCount < cost) return;
       
        _currentPerksCount = Mathf.Clamp(_currentPerksCount - cost, 0, _maxPerksCount);
        EventManager.Trigger(EventType.OnPerkChanged);
        Debug.Log(_currentPerksCount);
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

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        EventManager.Trigger(EventType.OnPerkChanged);
    }

    #region TEST
    [ContextMenu("Add Experience")]
    public void AddExperienceForce()
    {
        AddExperience(100);
    }

    [ContextMenu("Add perk")]
    public void ForceAddPerk()
    {
        AddPerk(1);
    }
    #endregion
}