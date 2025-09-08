using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SimpleTower : AbstractTower
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] public int damage = 10;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TargetRing targetRing;


    private float fireCountdown = 0f;
    private List<I_TestDamageable> enemiesInRange = new List<I_TestDamageable>();

    private void Awake()
    {
        
    }

    private void Update()
    {
        fireCountdown -= Time.deltaTime;

        if (enemiesInRange.Count == 0) return;


        if (fireCountdown <= 0f)
        {
            I_TestDamageable target = enemiesInRange[0];

            if (target != null)
            {
                MonoBehaviour mb = target as MonoBehaviour;
                if (mb != null)
                {
                    Transform targetTransform = mb.transform;
                    Shoot(target, targetTransform);
                    fireCountdown = 1f / fireRate;
                }
                else
                {
                    enemiesInRange.RemoveAt(0);
                    
                }
            }
            else
            {
                enemiesInRange.RemoveAt(0);
            }
        }
    }

    public override void Shoot(I_TestDamageable target, Transform targetTransform)
    {
        SimpleTowerBullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<SimpleTowerBullet>();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.damage = damage;
        bullet.SetTarget(target, targetTransform);
    }

    private void HandleEnemyDeath(I_TestDamageable deadEnemy)
    {
        if (enemiesInRange.Contains(deadEnemy))
        {
            enemiesInRange.Remove(deadEnemy);

            if (enemiesInRange.Count == 0)
            {
                targetRing.Hide();
            }
            else
            {
                I_TestDamageable nextTarget = enemiesInRange[0];
                MonoBehaviour mb = nextTarget as MonoBehaviour;
                if (mb != null)
                {
                    targetRing.RingActive(mb.transform);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        I_TestDamageable damageable = other.GetComponent<I_TestDamageable>();
        if (damageable != null && !enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Add(damageable);
            damageable.OnDeath += HandleEnemyDeath;

            if (enemiesInRange.Count == 1)
            {
                MonoBehaviour mb = damageable as MonoBehaviour;
                if (mb != null)
                {
                    targetRing.RingActive(mb.transform);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        I_TestDamageable damageable = other.GetComponent<I_TestDamageable>();
        if (damageable != null && enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Remove(damageable);
            damageable.OnDeath -= HandleEnemyDeath;
        }

        if (enemiesInRange.Count == 0)
        {
            targetRing.Hide();
        }
        else
        {
            // Mover el anillo al siguiente enemigo
            I_TestDamageable nextTarget = enemiesInRange[0];
            MonoBehaviour mb = nextTarget as MonoBehaviour;
            if (mb != null)
            {
                targetRing.RingActive(mb.transform);
            }
        }
    }


}

