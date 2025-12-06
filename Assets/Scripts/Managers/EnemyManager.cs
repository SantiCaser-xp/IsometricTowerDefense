using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingltonBase<EnemyManager>
{
    private List<MVC_Enemy> activeEnemies = new List<MVC_Enemy>();

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;

    public void RegisterEnemy(MVC_Enemy enemy)
    {
        if (!activeEnemies.Contains(enemy))
            activeEnemies.Add(enemy);
    }
    public void UnregisterEnemy(MVC_Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }
    public void NotifyTargetLost(ITargetable lostTarget)
    {
        foreach (var enemy in activeEnemies)
        {
            enemy.NotifyTargetLost(lostTarget);
        }
    }

    public List<MVC_Enemy> GetEnemiesInRange(Vector3 position, float range)
    {
        List<MVC_Enemy> enemiesInRange = new List<MVC_Enemy>();
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null && Vector3.Distance(position, enemy.transform.position) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    /*private void OnGUI()
    {
        if (showDebugInfo)
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Active Enemies: {activeEnemies.Count}");
        }
    }*/
}
