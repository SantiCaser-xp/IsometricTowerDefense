using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalStamina : MonoBehaviour, IObservable
{
    [SerializeField] float _timeToRecharge = 180f; //3 minutes
    [SerializeField] float _amountPerInterval = 1f;
    [SerializeField] float _maxStamina = 100f;
    [SerializeField] LocalizationTime _myLoc;
    float _currentStamina = 0f;
    public float CurrentStamina => _currentStamina;
    bool _recharging = false;

    DateTime _nextStaminaTime;
    DateTime _lastStaminaTime;
    TimeSpan _time;

    [SerializeField] string _titleNotif = "Stamina Recharged!";
    [SerializeField] string _messageNotif = "Your stamina has been fully recharged.";
    [SerializeField] IconSelecter _smallIcon = IconSelecter.icon_0;
    [SerializeField] IconSelecter _largeIcon = IconSelecter.icon_1;
    public int _id;

    List<IObserver> _observers = new List<IObserver>();

    void Start()
    {
        Load();

        if (_currentStamina < _maxStamina && !_recharging)
        {
            StartCoroutine(UpdateStaminaRoutine());
        }

        UpdateTimer();

        if (_currentStamina < _maxStamina)
        {
            _time = _nextStaminaTime - MyLocation(_myLoc);
            DisplayNotification();
        }
    }

    void DisplayNotification()
    {
        if (_currentStamina >= _maxStamina)
            return; // No programar notificación si ya está llena

        // Cancela la notificación anterior antes de crear una nueva
        ControladorNotificaciones.Instance.CancelNotification(_id);

        // Calcula el tiempo restante para llenar la stamina
        float secondsToFull = (_maxStamina - _currentStamina) * _timeToRecharge;
        DateTime fireTime = NextTime(MyLocation(_myLoc), secondsToFull);

        _id = ControladorNotificaciones.Instance.DisplayNotification(
            _titleNotif,
            _messageNotif,
            _smallIcon,
            _largeIcon,
            fireTime
        );
    }


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

            while (currentTime > nextTime)
            {
                if (_currentStamina == _maxStamina) break;

                _currentStamina++;
                addedStamina = true;

                UpdateTimer();
                DateTime timeToAdd = nextTime;

                if (_lastStaminaTime > nextTime) timeToAdd = _lastStaminaTime;

                nextTime = NextTime(timeToAdd, _timeToRecharge);
            }

            if (addedStamina)
            {
                _nextStaminaTime = nextTime;
                _lastStaminaTime = currentTime;
                Save();
            }

            UpdateTimer();

            yield return new WaitForEndOfFrame();
        }

        ControladorNotificaciones.Instance.CancelNotification(_id);
        _recharging = false;
    }

    void UpdateTimer()
    {
        if (_currentStamina > _maxStamina)
        {
            return;
        }

        _time = _nextStaminaTime - MyLocation(_myLoc);

        if (_time.TotalSeconds <= 0)
            _time = TimeSpan.Zero;

        Notify();
    }

    [ContextMenu("UseStaminaTest")]
    public void TakeStamina()
    {
        UseStamina(25);
    }

    public void AddStamina(int value)
    {
        if (value <= 0) return;
        _currentStamina += value;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        Save();
        Notify();
    }

    public bool UseStamina(int value)
    {
        if (_currentStamina - value >= 0)
        {
            _currentStamina -= value;

            Save();

            ControladorNotificaciones.Instance.CancelNotification(_id);

            if (_currentStamina < _maxStamina)
            {
                DisplayNotification();
            }

            if (!_recharging)
            {
                _nextStaminaTime = NextTime(MyLocation(_myLoc), _timeToRecharge);
                StartCoroutine(UpdateStaminaRoutine());
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    DateTime MyLocation(LocalizationTime loc)
    {
        switch (loc)
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
        if (string.IsNullOrEmpty(data))
            return MyLocation(_myLoc);

        DateTime result;
        if (DateTime.TryParse(data, out result))
            return result;
        else
            return MyLocation(_myLoc); // O puedes devolver DateTime.MinValue si prefieres
    }



    void Save()
    {
        var sd = SaveWithJSON.Instance._staminaData;

        sd.CurrentStamina = Mathf.Clamp(Mathf.RoundToInt(_currentStamina), 0, Mathf.RoundToInt(_maxStamina));
        sd.NextStaminaTime = _nextStaminaTime.ToString();
        sd.LastStaminaTime = _lastStaminaTime.ToString();
        sd.id = _id.ToString();


        SaveWithJSON.Instance._staminaData = sd;
        SaveWithJSON.Instance.SaveGame();
    }

    void Load()
    {
        var sd = SaveWithJSON.Instance._staminaData;

        _currentStamina = Mathf.Clamp(sd.CurrentStamina, 0, Mathf.RoundToInt(_maxStamina));
        _nextStaminaTime = StringToDateTime(sd.NextStaminaTime);
        _lastStaminaTime = StringToDateTime(sd.LastStaminaTime);

        // id es un número, no una fecha
        int parsedId;
        if (int.TryParse(sd.id, out parsedId))
            _id = parsedId;
        else
            _id = 0;
    }


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
            obs.UpdateData(_currentStamina, _maxStamina, _time);
        }
    }
    #endregion
}