using UnityEngine;

public class HeavyTurret : AbstractTower
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected AbstractFactory<AbstractGreanade> _factory;
    [SerializeField] protected BoxCollider boxCollider;
    [SerializeField] protected TowerHealthBar _healthBar;
    [SerializeField] protected GameObject normalVersion;
    [SerializeField] protected GameObject damagedVersion;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float _maxHeight = 5f;
    [SerializeField] protected float _flightDuration = 1.5f;
    private IDamageable<float> target;
    private Transform targetTransform;

    //    public float projectileSpeed = 10f; // velocidad “horizontal” del proyectil
    //public float projectileHeight = 5f;
    //public float range = 20f;
    //public float shootInterval = 2f;

    [Header("Rotation Settings")]
    [SerializeField] protected TowerMeshRotator _meshTopRotatior;

    [SerializeField] protected Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        _factory = GreanadeFactory.Instance;
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        if (enemiesInRange.Count == 0)
        {
            // idle mode
            _meshTopRotatior.RotateTowerIdle();
            _animator.SetBool("IsShooting", false);
        }
        else
        {

            // battle mode
            target = enemiesInRange[0];

            MonoBehaviour mb = target as MonoBehaviour;

            if (mb == null) { enemiesInRange.RemoveAt(0); return; }

            targetTransform = mb.transform;

            _meshTopRotatior.RotateTowerToEnemy(targetTransform);

            // // do fire if tower done look at target
            if (fireCountdown <= 0f)
            {
                _animator.SetBool("IsShooting", true);

                //Shoot(target, targetTransform);
                //ShootPredicted(targetTransform);
                fireCountdown = 1f / fireRate;
            }
        }
    }

    //Called by animation event
    public void CallShoot()
    {
        Debug.Log("Shoot called");
        Shoot(target, targetTransform);
    }

    protected override void Shoot(IDamageable<float> target, Transform targetTransform)
    {
        var bullet = _factory.Create();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Init(firePoint.position, targetTransform.position, _maxHeight, _flightDuration);
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

    //void ShootPredicted(Transform enemy)
    //{
    //    // Si el enemigo tiene Rigidbody o un script con velocidad
    //    Vector3 enemyVelocity = Vector3.zero;
    //    Rigidbody rb = enemy.GetComponent<Rigidbody>();
    //    if (rb != null)
    //        enemyVelocity = rb.velocity;

    //    // Calcular distancia horizontal
    //    Vector3 flatStart = new Vector3(firePoint.position.x, 0, firePoint.position.z);
    //    Vector3 flatTarget = new Vector3(enemy.position.x, 0, enemy.position.z);
    //    float distance = Vector3.Distance(flatStart, flatTarget);

    //    // Tiempo estimado de vuelo (distancia / velocidad proyectil)
    //    float timeToTarget = distance / projectileSpeed;

    //    // Predicción de posición futura
    //    Vector3 predictedPos = enemy.position + enemyVelocity * timeToTarget;

    //    // Crear y disparar proyectil
    //    GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    //    Greanade mp = p.GetComponent<Greanade>();

    //    // duración del vuelo = el mismo tiempo estimado
    //    mp.Init(firePoint.position, predictedPos, projectileHeight, timeToTarget);
    //}
}