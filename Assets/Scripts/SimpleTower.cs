using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : AbstractTower
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] public int damage = 10;
    [SerializeField] public float radius = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TargetRing targetRing;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask _enemyMask;
    //[SerializeField] private SphereCollider enemyDetectorCollider;
    [SerializeField] private TowerHealthBar _healthBar;
    [SerializeField] private GameObject normalVersion;
    [SerializeField] private GameObject damagedVersion;
    [SerializeField] private IsPlaced isPlaced;
    [SerializeField] private bool isDead = false;


    private float fireCountdown = 0f;
    private List<IDamageable<float>> enemiesInRange = new List<IDamageable<float>>();

    private void Awake()
    {
        isPlaced = GetComponent<IsPlaced>();

        _currentHealth = _maxHealth;
        _healthBar.SetHealth(_currentHealth, _maxHealth);
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider not found on SimpleTower.");
        }
        //enemyDetectorCollider = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        _factory = FactorySimpleBullet.Instance;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }

        if (isPlaced.isPlaced == false || isDead)
        {
            return;
        }

        fireCountdown -= Time.deltaTime;

        SphereCasting();
        UpdateRingPosition();

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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _healthBar.SetHealth(_currentHealth, _maxHealth);
    }

    public override void Die()
    {
        if (normalVersion != null) normalVersion.SetActive(false);
        if (damagedVersion != null) damagedVersion.SetActive(true);

        if (boxCollider != null) boxCollider.enabled = false;
        if (_healthBar != null) _healthBar.gameObject.SetActive(false);

        isDead = true;
    }

    public override void Shoot(IDamageable<float> target, Transform targetTransform)
    {
        var bullet = _factory.Create();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet._damage = damage;
        bullet.SetTarget(target, targetTransform);
        bullet._isShooted = true;
    }

    private void UpdateRingPosition()
    {
        while (enemiesInRange.Count > 0)
        {
            var target = enemiesInRange[0] as MonoBehaviour;
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                enemiesInRange.RemoveAt(0);

                if (enemiesInRange.Count > 0)
                {
                    var nextMb = enemiesInRange[0] as MonoBehaviour;
                    if (nextMb != null)
                        targetRing.RingActive(nextMb.transform);
                }
                else
                {
                    targetRing.Hide();
                }
            }
            else
            {
                break;
            }
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (enemyDetectorCollider != null && other != null)
        {
            if (!enemyDetectorCollider.enabled || !enemyDetectorCollider.isTrigger)
                return;

            {
                IDamageable<float> damageable = other.GetComponent<IDamageable<float>>();
            if (damageable != null && !enemiesInRange.Contains(damageable))
            {
                enemiesInRange.Add(damageable);

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
    }*/

    private void SphereCasting()
    {
        //if (enemiesInRange.Count < 1) return;

        Collider[] destruct = Physics.OverlapSphere(transform.position, radius, _enemyMask);
        Debug.Log("Detected");

        foreach (var item in destruct)
        {
            enemiesInRange.Add(item.GetComponent<IDamageable<float>>());
        }


        if (enemiesInRange.Count > 0)
        {
            MonoBehaviour mb = enemiesInRange[0] as MonoBehaviour;

            if (mb != null)
            {
                targetRing.RingActive(mb.transform);
            }
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        IDamageable<float> damageable = other.GetComponent< IDamageable<float>>();
        if (damageable != null && enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Remove(damageable);
        }

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
    }*/



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}