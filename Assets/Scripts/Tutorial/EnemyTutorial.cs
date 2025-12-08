using System.Collections;
using UnityEngine;

public class EnemyTutorial : MonoBehaviour
{
    [SerializeField] EnemyFactory _lowEnemyFactory;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] int[] _enemiesToSpawn;
    [SerializeField] float _delayBetweenSpawn = 1f;
    int _randomIndex;
    int _killedCount;
    int _totalEnemyCount;
    bool _isSpawned = false;
    bool _isCatched = false;

    void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>();

        for (int i = 0; i < _enemiesToSpawn.Length; i++)
        {
            _totalEnemyCount += _enemiesToSpawn[i];
        }
    }

    void OnEnable()
    {
        EventManager.Subscribe(EventType.PlaceBuilding, SpawnOneEnemyRoutine);
        EventManager.Subscribe(EventType.OnEnemyKilled, EnemyKilled);
        EventManager.Subscribe(EventType.FirstGoldCatched, SpawnEnemiesRoutine);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.PlaceBuilding, SpawnOneEnemyRoutine);
        EventManager.Unsubscribe(EventType.OnEnemyKilled, EnemyKilled);
        EventManager.Unsubscribe(EventType.FirstGoldCatched, SpawnEnemiesRoutine);
    }

    public void EnemyKilled(params object[] args)
    {
        _killedCount++;

        if (_killedCount >= _totalEnemyCount)
        {
            EventManager.Trigger(EventType.OnGameWin);
            Debug.Log("Win");
        }
    }

    void SpawnOneEnemyRoutine(params object[] args)
    {
        if (_isSpawned) return;

        StartCoroutine(SpawnOneEnemy());

    }

    void SpawnEnemiesRoutine(params object[] args)
    {
        if(_isCatched) return;

        StartCoroutine(SpawnEnemies());
    }

    public void Spawn(EnemyFactory currFactory)
    {
        var enemy = currFactory.Create();

        enemy.transform.position = _spawnPoints[_randomIndex].position;
    }

    IEnumerator SpawnOneEnemy()
    {
        _isSpawned = true;

        for (int i = 0; i < _enemiesToSpawn[0]; i++)
        {
            _randomIndex = Random.Range(1, _spawnPoints.Length);
            Spawn(_lowEnemyFactory);
            yield return new WaitForSeconds(_delayBetweenSpawn);
        }

        yield return null;
    }

    IEnumerator SpawnEnemies()
    {
        _isCatched = true;

        for (int i = 0; i < _enemiesToSpawn[1]; i++)
        {
            _randomIndex = Random.Range(1, _spawnPoints.Length);
            Spawn(_lowEnemyFactory);
            yield return new WaitForSeconds(_delayBetweenSpawn);
        }

        yield return null;
    }
}