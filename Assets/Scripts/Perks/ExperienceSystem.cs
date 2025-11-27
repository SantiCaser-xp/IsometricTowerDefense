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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
        EventManager.Subscribe(EventType.OnGameWin, Save);
        EventManager.Subscribe(EventType.OnGameOver, OnGameLose);
    }

    void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += UpdateData;

        Load();
        Notify();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
        EventManager.Unsubscribe(EventType.OnGameWin, Save);
        EventManager.Unsubscribe(EventType.OnGameOver, OnGameLose);
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

        Notify();
    }

    void AddLevel()
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
        Save();
    }

    void RecalculateExperienceThreshold()
    {
        _currentExperienceThreshold *= _experienceGainMultiplier;
    }

    public void UpdateData(ConfigResponse configResponse)
    {
        var experienceToAdd = RemoteConfigService.Instance.appConfig.GetFloat("XpToAdd");
        AddExperience(experienceToAdd);
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        EventManager.Trigger(EventType.OnPerkChanged);
        OnGameLose(scene, mode);
    }

    void Save(params object[] parameters)
    {
        if (SaveWithJSON.Instance == null) return;

        var sd = SaveWithJSON.Instance._experienceData;

        sd.CurrentExperience = _currentExperience;
        sd.CurrentExperienceThreshold = _currentExperienceThreshold;
        sd.CurrentLevel = _currentLevel;
        sd.CurrentPerks = _currentPerksCount;

        SaveWithJSON.Instance._experienceData = sd;
        SaveWithJSON.Instance.SaveGame();
    }

    void Load()
    {
        var sd = SaveWithJSON.Instance._experienceData;

        _currentExperience = Mathf.Clamp(sd.CurrentExperience, 0f, _maxExperienceLimit);
        _currentExperienceThreshold = (sd.CurrentExperienceThreshold > 0f) ? sd.CurrentExperienceThreshold : _startExperienceThreshold;
        _currentLevel = Mathf.Clamp(sd.CurrentLevel, _startLevel, _maxLevel);
        _currentPerksCount = Mathf.Clamp(sd.CurrentPerks, 0, _maxPerksCount);
    }

    void OnGameLose(params object[] parameters)
    {
        if (SaveWithJSON.Instance == null) return;

        SaveWithJSON.Instance.LoadGame();

        Load();
        Notify();
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

    #region Observable
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

    public void Notify()
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentExperience, _currentExperienceThreshold, _currentLevel);
        }
    }
    #endregion
}