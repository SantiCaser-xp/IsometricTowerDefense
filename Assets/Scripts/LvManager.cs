//using System;
using System.Collections.Generic;
using UnityEngine;

public class LvManager : MonoBehaviour, IObservable
{
    /*public event Action OnGameOver;
    public event Action OnWin;*/

    [SerializeField] private PlayerBase playerBase;
    [SerializeField] private int _enemiesToKill;
    private int _enemiesKilled;
    private List<IObserver> _observers = new List<IObserver>();

    void Awake()
    {
        if (playerBase != null)
        {
            playerBase.OnPlayerBaseDestroyed += OnPlayerBaseDiedHandler;
        }

        MVC_Enemy.OnEnemyKilled += EnemyKilled;
    }

    void Start()
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData(_enemiesKilled, _enemiesToKill);
        }
    }

    private void OnDestroy()
    {
        MVC_Enemy.OnEnemyKilled -= EnemyKilled;
    }

    public void EnemyKilled()
    {
        _enemiesKilled++;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_enemiesKilled, _enemiesToKill);
        }

        if (_enemiesKilled >= _enemiesToKill)
        {
            //Debug.Log("All Enemies Killed! You Win!");
            NotifyGameStatus(GameStatus.Win);
        }
    }

    private void NotifyGameStatus(GameStatus status)
    {
        foreach (var obs in _observers)
        {
            obs.UpdateGameStatus(status);
        }
    }

    private void OnPlayerBaseDiedHandler()
    {
        Debug.Log("Player Base Destroyed! Game Over!");
        NotifyGameStatus(GameStatus.Lose);
    }

    private void OnDisable()
    {
        if (playerBase != null)
        {
            playerBase.OnPlayerBaseDestroyed -= OnPlayerBaseDiedHandler;
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