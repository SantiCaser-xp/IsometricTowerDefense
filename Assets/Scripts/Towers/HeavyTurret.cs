using UnityEngine;

public class HeavyTurret : AbstractTower
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected AbstractFactory<AbstractBullet> _factory;
    [SerializeField] protected TargetRing targetRing;
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] protected TowerHealthBar _healthBar;
    [SerializeField] protected GameObject normalVersion;
    [SerializeField] protected GameObject damagedVersion;
    [SerializeField] protected GameObject projectilePrefab;

    [Header("Rotation Settings")]
    [SerializeField] protected TowerMeshRotator _meshTopRotatior;

    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        _factory = FactorySimpleBullet.Instance;
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        if (enemiesInRange.Count == 0)
        {
            // idle mode
            _meshTopRotatior.RotateTowerIdle();
        }
        else
        {
            // battle mode
            IDamageable<float> target = enemiesInRange[0];

            MonoBehaviour mb = target as MonoBehaviour;

            if (mb == null) { enemiesInRange.RemoveAt(0); return; }

            Transform targetTransform = mb.transform;

            _meshTopRotatior.RotateTowerToEnemy(targetTransform);

            // // do fire if tower done look at target
            if (fireCountdown <= 0f)
            {
                Shoot(target, targetTransform);
                fireCountdown = 1f / fireRate;
            }
        }
    }

    protected override void Shoot(IDamageable<float> target, Transform targetTransform)
    {
        //var bullet = _factory.Create();
        //bullet.transform.position = firePoint.position;
        //bullet.transform.rotation = firePoint.rotation;
        //bullet.SetTarget(target, targetTransform);
        GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Greanade mp = p.GetComponent<Greanade>();
        mp.Init(firePoint.position, targetTransform.position, 5f, 1.5f);
        // 5f = altura m�xima, 1.5f = duraci�n del vuelo
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
}