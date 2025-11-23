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

    protected override void Start()
    { 
        base.Start();
        RemoteConfigService.Instance.FetchCompleted += UpdateData;
    }

    private void OnDisable()
    {
        RemoteConfigService.Instance.FetchCompleted -= UpdateData;
    }

    public void UpdateData(ConfigResponse configResponse)
    {
        spawnEnemy = RemoteConfigService.Instance.appConfig.GetBool("SpawnEnemy");

        var _secondsToSpawn = RemoteConfigService.Instance.appConfig.GetFloat("EnemySpawnerTimer");

        var newPosition = RemoteConfigService.Instance.appConfig.GetInt("EnemySpawnerPosition");

        if (newPosition == 0)
        {
            transform.localPosition = new Vector3(0, 0, -0);
        }
        else if (newPosition == 1)
        {
            transform.localPosition = new Vector3(-37f, 0, -69f);
        }
        else if (newPosition == 2)
        {
            transform.localPosition = new Vector3(37f, 0, -69f);
        }
        else if (newPosition == 3)
        {
            transform.localPosition = new Vector3(0f, 0, -125f);
        }
        else
        {
            Debug.Log("No valid position found for Enemy Spawner");
        }


            _secondToSpawn = _secondsToSpawn;

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
