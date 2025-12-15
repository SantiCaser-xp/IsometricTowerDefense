using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingltonBase<EnemyManager>
{
    public List<MVC_Enemy> ActiveEnemies { get; private set; } = new List<MVC_Enemy>();

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;

    public void RegisterEnemy(MVC_Enemy enemy)
    {
        if (!ActiveEnemies.Contains(enemy))
            ActiveEnemies.Add(enemy);
    }
    public void UnregisterEnemy(MVC_Enemy enemy)
    {
        ActiveEnemies.Remove(enemy);
    }
    public void NotifyTargetLost(ITargetable lostTarget)
    {
        foreach (var enemy in ActiveEnemies)
        {
            enemy.NotifyTargetLost(lostTarget);
        }
    }

    //public List<MVC_Enemy> GetEnemiesInRange(Vector3 position, float range)
    //{
    //    List<MVC_Enemy> enemiesInRange = new List<MVC_Enemy>();
    //    foreach (var enemy in ActiveEnemies)
    //    {
    //        if (enemy != null && Vector3.Distance(position, enemy.transform.position) <= range)
    //        {
    //            enemiesInRange.Add(enemy);
    //        }
    //    }
    //    return enemiesInRange;
    //}
    /*private void OnGUI()
    {
        if (showDebugInfo)
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Active Enemies: {activeEnemies.Count}");
        }
    }*/
}
