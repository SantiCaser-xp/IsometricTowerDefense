using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTower : Destructible
{
    [Header("Tower Settings")]
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float radius = 10f;
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected LayerMask _enemyMask;
    protected float fireCountdown = 0f;
    protected List<IDamageable<float>> enemiesInRange = new List<IDamageable<float>>();

    protected virtual void Awake()
    {
        _currentHealth = _maxHealth;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }
    }

    protected virtual void Update()
    {
        fireCountdown -= Time.deltaTime;

        SphereCasting();
    }

    protected virtual void SphereCasting()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, _enemyMask);
        HashSet<IDamageable<float>> detectedEnemies = new HashSet<IDamageable<float>>();


        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<IDamageable<float>>();

            if (enemy != null)
            {
                detectedEnemies.Add(enemy);
                if (!enemiesInRange.Contains(enemy))
                    enemiesInRange.Add(enemy);
            }
        }

        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            var enemy = enemiesInRange[i] as MonoBehaviour;
            if (enemy == null || !enemy.gameObject.activeInHierarchy || !detectedEnemies.Contains(enemiesInRange[i]))
            {
                enemiesInRange.RemoveAt(i);
            }
        }
    }

    protected virtual void Shoot(IDamageable<float> target, Transform targetTransform) { }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}