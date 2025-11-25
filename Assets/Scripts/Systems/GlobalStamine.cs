using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStamine : MonoBehaviour, IObservable
{
    [SerializeField] private float _timeToRecharge = 180f; //3 minutes
    [SerializeField] private float _amountPerInterval = 1f;
    [SerializeField] private float _maxStamina = 100f;
    private float _currentStamina = 0f;
    bool _recharging = false;

    DateTime _nextStaminaTime;
    DateTime _lastStaminaTime;

    [SerializeField] LocalizationTime _myLoc;

    private List<IObserver> _observers = new List<IObserver>();

    void Awake()
    {
        //LoadFromSave();
        if (_currentStamina < _maxStamina && !_recharging)
        {
            StartCoroutine(UpdateStaminaRoutine());
        }
    }

    private void Start()
    {
        UpdateTimer();
    }

    //void OnApplicationFocus(bool focus)
    //{
    //    if(!focus) SaveToSaveData();
    //}

    IEnumerator UpdateStaminaRoutine()
    {
        _recharging = true;

        UpdateTimer();

        DateTime currentTime;
        DateTime nextTime;
        bool addedStamina = false;

        while (_currentStamina < _maxStamina)
        {
            currentTime = MyLocation(_myLoc);
            nextTime = _nextStaminaTime;
            addedStamina = false;

            while(currentTime > nextTime)
            {
                if (_currentStamina > _maxStamina) break;

                _currentStamina++;
                addedStamina = true;

                //SaveToSaveData();

                UpdateTimer();
                DateTime timeToAdd = nextTime;

                if(_lastStaminaTime > nextTime) timeToAdd = _lastStaminaTime;

                nextTime = NextTime(timeToAdd, _timeToRecharge);
            }

            if(addedStamina)
            {
                _nextStaminaTime = nextTime;
                _lastStaminaTime = currentTime;
            }

            //SaveToSaveData();
            UpdateTimer();

            yield return new WaitForEndOfFrame();
        }


        _recharging = false;
    }

    void UpdateTimer()
    {
        if (_currentStamina > _maxStamina)
        {
            return;
        }

        TimeSpan time = _nextStaminaTime - MyLocation(_myLoc);

        if (time.TotalSeconds <= 0)
            time = TimeSpan.Zero;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentStamina, _maxStamina, time);
        }
    }

    [ContextMenu("UseStaminaTest")]
    public void TakeStamina()
    {
        UseStamina(25);
    }

    public bool UseStamina(int value)
    {
        if(_currentStamina - value >= 0)
        {
            _currentStamina -= value;

            //SaveToSaveData();

            if(!_recharging)
            {
                _nextStaminaTime = NextTime(MyLocation(_myLoc), _timeToRecharge);
                StartCoroutine(UpdateStaminaRoutine());
            }
            return true;
        }
        else
        {
            Debug.Log($"Fuck off vagabuncha");
            return false;
        }
    }

    //void SaveToSaveData()
    //{
    //    SaveWithJSON.Instance._saveData.CurrentStamina = (int)_currentStamina;
    //    SaveWithJSON.Instance._saveData.NextStaminaTime = _nextStaminaTime.ToString();
    //    SaveWithJSON.Instance._saveData.LastStaminaTime = _lastStaminaTime.ToString();

    //    SaveWithJSON.Instance.SaveGame();
    //}

    //void LoadFromSave()
    //{
    //    _currentStamina = SaveWithJSON.Instance._saveData.CurrentStamina;

    //    _nextStaminaTime = StringToDateTime(SaveWithJSON.Instance._saveData.NextStaminaTime);
    //    _lastStaminaTime = StringToDateTime(SaveWithJSON.Instance._saveData.LastStaminaTime);
    //}

    DateTime MyLocation(LocalizationTime loc)
    {
        switch(loc)
        {
            case LocalizationTime.Local:
                return DateTime.Now;
            default:
                return DateTime.UtcNow;
        }
    }

    DateTime NextTime(DateTime time, float duration)
    {
        return time.AddSeconds(duration);
    }

    DateTime StringToDateTime(string data)
    {
        if(string.IsNullOrEmpty(data)) return MyLocation(_myLoc);
        else  return DateTime.Parse(data);
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
}