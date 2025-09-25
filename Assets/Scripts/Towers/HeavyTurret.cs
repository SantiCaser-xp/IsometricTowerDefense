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
    [SerializeField] protected IsPlaced isPlaced;

    [Header("Rotation Settings")]
    [SerializeField] protected TowerMeshRotator _meshTopRotatior;

    protected override void Awake()
    {
        base.Awake();
        isPlaced = GetComponent<IsPlaced>();
        boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        _factory = FactorySimpleBullet.Instance;
    }

    protected override void Update()
    {
        base.Update();
        UpdateRingPosition();
        if (isPlaced.isPlaced == false || isDead) return;

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

    protected virtual void UpdateRingPosition()
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
}