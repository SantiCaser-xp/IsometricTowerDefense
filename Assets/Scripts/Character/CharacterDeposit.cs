using System.Collections.Generic;
using UnityEngine;

public class CharacterDeposit : MonoBehaviour, IObservable
{
    [SerializeField] private int _maxGold = 9999;
    private int _currentGold = 0;
    public int CurrentGold => _currentGold;
    private int _startedDeposit = 5;
    List<IObserver> _observers = new List<IObserver>();

    private void Start()
    {
        _currentGold = _startedDeposit;

        foreach(var obs in _observers)
        {
            obs.UpdateData(_currentGold);
        }
    }

    public void AddDeposit(int amount)
    {
        if (amount <= 0) return;

        _currentGold += amount;

        _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentGold);
        }
    }

    public void SubstructDeposit(int amount)
    {
        if (amount <= 0) return;

        _currentGold -= amount;

        _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentGold);
        }
    }

    public void ChangeStartedDeposit(int amount)
    {
        _startedDeposit += amount;
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

    #region Test
    [ContextMenu("Add Gold 50")]
    public void TakeGold()
    {
        AddDeposit(50);
    }
    #endregion
}