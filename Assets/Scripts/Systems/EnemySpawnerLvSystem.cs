using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerLvSystem : MonoBehaviour, IObservable
{
    [SerializeField] private Animator _sign;

    [SerializeField] private EnemyFactory _lowEnemyFactory;
    [SerializeField] private EnemyFactory _midEnemyFactory;
    [SerializeField] private EnemyFactory _hardEnemyFactory;

    [SerializeField] private Transform _upZone;
    [SerializeField] private Transform _downZone;
    [SerializeField] private Transform _rightZone;
    [SerializeField] private Transform _leftZone;

    [SerializeField] private Transform _currentZone;

    [SerializeField] private int _enemiesToSpawn;

    private List<IObserver> _observers = new List<IObserver>();
    [SerializeField] private int _enemiesToKill;
    private int _enemiesKilled;

    public event Action AllWavesCleared;

    private void Awake()
    {
        MVC_Enemy.OnEnemyKilled += EnemyKilled;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    public void Spawn(EnemyFactory currFactory)
    {
        var enemy = currFactory.Create();
        //enemy.transform.position = _currentZone.position;
        enemy.transform.position = new Vector3 (_currentZone.position.x + UnityEngine.Random.Range(3,6),
            _currentZone.position.y,
            _currentZone.position.z + UnityEngine.Random.Range(3, 6));
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

        if (_enemiesKilled >= 5)
        {
            Debug.Log("All Enemies Killed! You Win!");
            AllWavesCleared?.Invoke();
            //NotifyGameStatus(GameStatus.Win);
        }
    }

    private void NotifyGameStatus(GameStatus status)
    {
        foreach (var obs in _observers)
        {
            obs.UpdateGameStatus(status);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3f);
        _sign.SetTrigger("RingRight");
        _currentZone = _rightZone;
        _enemiesToSpawn = 5;
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            Spawn(_lowEnemyFactory);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(10f);


        _sign.SetTrigger("RingDown");
        _currentZone = _downZone;
        _enemiesToSpawn = 7;
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            Spawn(_midEnemyFactory);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(10f);

        _sign.SetTrigger("RingLeft");
        _currentZone = _leftZone;
        _enemiesToSpawn = 10;
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            Spawn(_hardEnemyFactory);
            yield return new WaitForSeconds(1f);
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
