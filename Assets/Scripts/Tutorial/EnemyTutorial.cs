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
        }
    }

    void SpawnOneEnemyRoutine(params object[] args)
    {
        StartCoroutine(SpawnEnemies(_enemiesToSpawn[0]));
    }

    void SpawnEnemiesRoutine(params object[] args)
    {
        StartCoroutine(SpawnEnemies(_enemiesToSpawn[1]));
    }

    public void Spawn(EnemyFactory currFactory)
    {
        var enemy = currFactory.Create();

        enemy.transform.position = _spawnPoints[_randomIndex].position;
    }

    IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _randomIndex = Random.Range(1, _spawnPoints.Length);
            Spawn(_lowEnemyFactory);
            yield return new WaitForSeconds(_delayBetweenSpawn);
        }

        yield return null;
    }
}