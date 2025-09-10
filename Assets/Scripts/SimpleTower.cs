using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : AbstractTower
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] public int damage = 10;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TargetRing targetRing;


    private float fireCountdown = 0f;
    private List<IDamageable<float>> enemiesInRange = new List<IDamageable<float>>();

    private void OnEnable()
    {
        
    }
    private void Start()
    {
        _factory = FactorySimpleBullet.Instance;
        
    }

    private void Update()
    {
        fireCountdown -= Time.deltaTime;

        if (enemiesInRange.Count == 0) return;


        if (fireCountdown <= 0f)
        {
            IDamageable<float> target = enemiesInRange[0];

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

    public override void Shoot(IDamageable<float> target, Transform targetTransform)
    {
        var bullet = _factory.Create();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        //bullet._damage = damage;
        //bullet.SetTarget(target, targetTransform);
        //bullet._isShooted = true;
    }

    private void HandleEnemyDeath(IDamageable<float> deadEnemy)
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
                IDamageable<float> nextTarget = enemiesInRange[0];
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
        IDamageable<float> damageable = other.GetComponent<IDamageable<float>>();
        if (damageable != null && !enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Add(damageable);
            //damageable.OnDead += HandleEnemyDeath;

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
        IDamageable<float> damageable = other.GetComponent< IDamageable<float>>();
        if (damageable != null && enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Remove(damageable);
            //damageable.OnDeath -= HandleEnemyDeath;
        }

        if (enemiesInRange.Count == 0)
        {
            targetRing.Hide();
        }
        else
        {
            // Mover el anillo al siguiente enemigo
            IDamageable<float> nextTarget = enemiesInRange[0];
            MonoBehaviour mb = nextTarget as MonoBehaviour;
            if (mb != null)
            {
                targetRing.RingActive(mb.transform);
            }
        }
    }


}

