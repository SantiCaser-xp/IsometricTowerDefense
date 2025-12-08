using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class CharacterDeposit : MonoBehaviour, IObservable
{
    private int _maxGold = 9999;
    private int _currentGold = 0;
    public int CurrentGold => _currentGold;
    [SerializeField] bool _tutorialMode = false;
    //private int _savedGold = 0;
    //private int _startedDeposit = 5;
    List<IObserver> _observers = new List<IObserver>();

    private void Start()
    {
        _currentGold = PerkSkillManager.Instance.StartGold;
        if (_tutorialMode) _currentGold = 2;
        Notify();

        //RemoteConfigService.Instance.FetchCompleted += UpdateData;
    }

    public void AddDeposit(int amount)
    {
        if (amount <= 0) return;

        _currentGold += amount;

        _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);

        Notify();
    }

    public void SubstructDeposit(int amount)
    {
        if (amount <= 0) return;

        _currentGold -= amount;

        _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);

        Notify();
    }

    //public void ChangeStartedDeposit(int amount)
    //{
    //    _startedDeposit += amount;
    //}

    //public void UpdateData(ConfigResponse configResponse)
    //{
    //    var hasCero = RemoteConfigService.Instance.appConfig.GetBool("SetMoneyTo0");

    //    if (hasCero)
    //    {
    //        _savedGold = _currentGold;
    //        _currentGold = 0;
    //    }
    //    else
    //    {
    //        _currentGold += _savedGold;
    //        _savedGold = 0;
    //    }

    //    Notify();
    //}

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
            obs.UpdateData(_currentGold);
        }
    }
    #endregion

    #region Test
    [ContextMenu("Add Gold 50")]
    public void TakeGold()
    {
        AddDeposit(50);
    }
    #endregion
}