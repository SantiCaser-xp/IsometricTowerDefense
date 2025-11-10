using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerLvSystem : MonoBehaviour
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



    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    public void Spawn(EnemyFactory currFactory)
    {
        var enemy = currFactory.Create();
        //enemy.transform.position = _currentZone.position;
        enemy.transform.position = new Vector3 (_currentZone.position.x + Random.Range(3,6),
            _currentZone.position.y,
            _currentZone.position.z + Random.Range(3, 6));
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
}
