using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : AbstractTower
{
    [Header("Tower Settings")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] public int damage = 10;
    [SerializeField] public float radius = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TargetRing targetRing;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private TowerHealthBar _healthBar;
    [SerializeField] private GameObject normalVersion;
    [SerializeField] private GameObject damagedVersion;
    [SerializeField] private IsPlaced isPlaced;
    [SerializeField] private bool isDead = false;

    [Header("Rotation Settings")]
    [SerializeField] private TowerMeshRotator _meshTopRotatior;
    
    private float fireCountdown = 0f;
    private List<IDamageable<float>> enemiesInRange = new List<IDamageable<float>>();

    private void Awake()
    {
        isPlaced = GetComponent<IsPlaced>();
        boxCollider = GetComponent<BoxCollider>();

        _currentHealth = _maxHealth;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);

        }
    }

    private void Start()
    {
        _factory = FactorySimpleBullet.Instance;
    }

    private void Update()
    {
        if (isPlaced.isPlaced == false || isDead) return;

        fireCountdown -= Time.deltaTime;

        SphereCasting();
        UpdateRingPosition();

        if (enemiesInRange.Count == 0)
        {
            // idle режим
            _meshTopRotatior.RotateTowerIdle();
        }
        else
        {
            // боевой режим
            IDamageable<float> target = enemiesInRange[0];

            MonoBehaviour mb = target as MonoBehaviour;

            if (mb == null) { enemiesInRange.RemoveAt(0); return; }

            Transform targetTransform = mb.transform;

            _meshTopRotatior.RotateTowerToEnemy(targetTransform);

            // стреляем только если башня уже почти смотрит на врага
            if (fireCountdown <= 0f && _meshTopRotatior.IsFacingTarget(targetTransform))
            {
                Shoot(target, targetTransform);
                fireCountdown = 1f / fireRate;
            }
        }
    }

    protected override void Shoot(IDamageable<float> target, Transform targetTransform)
    {
        var bullet = _factory.Create();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetTarget(target, targetTransform);
    }

    public override void Die()
    {
        EnemyTargetManager.Instance?.UnregisterTarget(this);

        if (normalVersion != null) normalVersion.SetActive(false);
        if (damagedVersion != null) damagedVersion.SetActive(true);

        if (boxCollider != null) boxCollider.enabled = false;
        if (_healthBar != null) _healthBar.gameObject.SetActive(false);
        isDead = true;
    }

    private void SphereCasting()
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

    private void UpdateRingPosition()
    {
        if (enemiesInRange.Count > 0)
        {
            MonoBehaviour mb = enemiesInRange[0] as MonoBehaviour;
            if (mb != null)
                targetRing.RingActive(mb.transform);
        }
        else
        {
            targetRing.Hide();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}