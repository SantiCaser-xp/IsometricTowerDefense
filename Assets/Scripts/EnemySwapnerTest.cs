using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwapnerTest : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        for (int i = 0; i < 10; i++)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
