using Unity.Services.RemoteConfig;
using UnityEngine;

public class EnemySpawner : GenericSpawner<EnemyFactory>
{
    public bool spawnEnemy;
    protected override void Spawn()
    {
        var enemy = _factory.Create();
        enemy.transform.position = _spawnPoints[0].position;
    }

    private void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += UpdateData;
    }

    public void UpdateData(ConfigResponse configResponse)
    {
        spawnEnemy = RemoteConfigService.Instance.appConfig.GetBool("SpawnEnemy");

        if (spawnEnemy)
        {
            InvokeRepeating(nameof(Spawn), 0, _secondToSpawn);
        }
        else
        {
            CancelInvoke(nameof(Spawn));
        }
    }
}
