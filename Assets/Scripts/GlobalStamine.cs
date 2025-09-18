using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStamine : MonoBehaviour, IObservable
{
    #region Singlton
    public static GlobalStamine Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _currentStamine = _maxStamine;
    }

    #endregion

    [SerializeField] private float _delayInSeconds = 900f; //15 minutes
    [SerializeField] private float _amountPerInterval = 5f;
    [SerializeField] private float _maxStamine = 100f;
    private float _currentStamine = 0f;
    private Coroutine _currentCoroutine;
    private List<IObserver> _observers = new List<IObserver>();

    private void Start()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(nameof(RestoreHealthRoutine));
        }

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentStamine, _maxStamine);
            obs.UpdateData(0);
        }
    }

    /// <summary>
    /// This is the method for generate health as tries for play.(Limit)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreHealthRoutine()
    {
        while (_currentStamine < _maxStamine)
        {
            yield return new WaitForSeconds(_delayInSeconds);

            _currentStamine += _amountPerInterval;
            _currentStamine = Mathf.Clamp(_currentStamine, 0f, _maxStamine);

            yield return null;
        }

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentStamine, _maxStamine);
            obs.UpdateData(0);
        }
    }

    public void SubstructHealth(float value)
    {
        if (value <= 0f) return;

        _currentStamine -= value;
        _currentStamine = Mathf.Clamp(_currentStamine, 0f, _maxStamine);

        if(_currentStamine < _maxStamine && _currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(nameof(RestoreHealthRoutine));
        }

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentStamine, _maxStamine);
            obs.UpdateData(0);
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
}