using System.Collections.Generic;
using UnityEngine;

public class CharacterDeposit : MonoBehaviour, IObservable
{
    [SerializeField] private int _maxGold = 9999;
    private int _currentGold = 0;
    List<IObserver> _observers = new List<IObserver>();

    private void Start()
    {
        foreach(var obs in _observers)
        {
            obs.UpdateData(_currentGold);
        }
    }

    public void ChangeDeposit(int amount)
    {
        if (amount <= 0) return;

        _currentGold += amount;

        _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentGold);
        }
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
        ChangeDeposit(50);
    }
    #endregion
}