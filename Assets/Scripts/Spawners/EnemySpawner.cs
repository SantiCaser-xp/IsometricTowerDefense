using UnityEngine;

public class EnemySpawner : GenericSpawner<EnemyFactory>
{
    protected override void Spawn()
    {
        var enemy = _factory.Create();
        enemy.transform.position = _spawnPoints[0].position;
    }
}
