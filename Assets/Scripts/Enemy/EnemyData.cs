using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Stats")]
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public float searchInterval;

    [Header("Flocking Settings")]
    public float separationRadius;
    public float separationWeight;
    public float flockUpdateInterval;
}
