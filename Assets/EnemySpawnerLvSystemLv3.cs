using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerLvSystemLv3 : MonoBehaviour, IObservable
{
    [SerializeField] private Animator _sign;

    [SerializeField] private EnemyFactory _kamikazeEnemyFactory; 
    [SerializeField] private EnemyFactory _midEnemyFactory;
    [SerializeField] private EnemyFactory _rangeEnemyFactory;

    [SerializeField] private Transform _upZone; 
    [SerializeField] private Transform _downZone;
    [SerializeField] private Transform _rightZone;
    [SerializeField] private Transform _leftZone;

    [SerializeField] private Transform _currentZone;

    [SerializeField] private int[]  _enemiesToSpawn;

    private List<IObserver> _observers = new List<IObserver>();
    [SerializeField] private int _enemiesToKill;
    private int _enemiesKilled;
    private int _enemiesTotal;

    [SerializeField] float _spawnDelayPerEnemy = 1f;

    private void Awake()
    {
        MVC_Enemy.OnEnemyKilled += EnemyKilled;
    }

    private void Start()
    {
        for (int i = 0; i < _enemiesToSpawn.Length; i++)
        {
            _enemiesTotal += _enemiesToSpawn[i];
        }

        Notify();

        StartCoroutine(SpawnEnemies());
    }

    public void Spawn(EnemyFactory currFactory)
    {
        var enemy = currFactory.Create();

        enemy.transform.position = new Vector3(_currentZone.position.x,
            _currentZone.position.y,
            _currentZone.position.z);
    }

    private void OnDestroy()
    {
        MVC_Enemy.OnEnemyKilled -= EnemyKilled;
    }

    public void EnemyKilled()
    {
        _enemiesKilled++;

        Notify();

        if (_enemiesKilled >= _enemiesTotal)
        {
            EventManager.Trigger(EventType.OnGameWin);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3f);
        _sign.SetTrigger("RingUp");
        _currentZone = _upZone;
        for (int i = 0; i < _enemiesToSpawn[0]; i++)
        {
            yield return new WaitForSeconds(_spawnDelayPerEnemy);
            Spawn(_kamikazeEnemyFactory);
        }
        yield return new WaitForSeconds(5f);


        _sign.SetTrigger("RingDown");
        _currentZone = _downZone;
        for (int i = 0; i < _enemiesToSpawn[1]; i++)
        {
            Spawn(_rangeEnemyFactory);
            yield return new WaitForSeconds(_spawnDelayPerEnemy);
        }
        yield return new WaitForSeconds(10f);


        _sign.SetTrigger("RingLeft");
        _currentZone = _leftZone;
        for (int i = 0; i < _enemiesToSpawn[2]; i++)
        {
            Spawn(_midEnemyFactory);
            yield return new WaitForSeconds(_spawnDelayPerEnemy);
        }
        yield return new WaitForSeconds(10f);


        _sign.SetTrigger("RingRight");
        _currentZone = _rightZone;
        
        for (int i = 0; i < _enemiesToSpawn[3]; i++)
        {
            Spawn(_rangeEnemyFactory);
            yield return new WaitForSeconds(_spawnDelayPerEnemy);
        }
        for (int i = 0; i < _enemiesToSpawn[4]; i++)
        {
            Spawn(_kamikazeEnemyFactory);
            yield return new WaitForSeconds(_spawnDelayPerEnemy);
        }
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
            obs.UpdateData(_enemiesKilled, _enemiesTotal);
        }
    }
    #endregion
}