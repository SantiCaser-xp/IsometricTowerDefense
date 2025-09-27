using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class BaseEnemy : Destructible
{
    [Header("Enemy Stats")]
    [SerializeField] public float searchInterval = 0.5f;
    [SerializeField] public float _experienceModidier = 3f;
    [SerializeField] float idleTime;
    //[SerializeField] public float detectionRadius;
    //[SerializeField] public LayerMask targetMask;
    //[SerializeField] public float _energy;

    [Header("Targeting")]
    [SerializeField] protected TargetingStrategy targetingStrategy = TargetingStrategy.Nearest;
    public ITargetable currentTarget;
    [SerializeField] Transform[] waypoints;
    [SerializeField] int currentWaypoint;
    public float lastAttackTime;
    public float lastSearchTime;



    [Header("Components")]
    [SerializeField] private GoldResourseFactory _goldFactory;
    public NavMeshAgent agent;
    protected Animator animator;
    protected ObjectPool<BaseEnemy> _myPool;
    protected CharacterDeposit _deposit;
    [SerializeField] public EnemyData data;

    public GameObject _target
    {
        get; private set;
    }


    private EnemyFSM<EnemyFSMStates, BaseEnemy> _enemyFSM;

    public int maxWaypoints
    {
        get
        {
            return waypoints.Length;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.walkSpeed;

        animator = GetComponent<Animator>();



        _enemyFSM = new EnemyFSM<EnemyFSMStates, BaseEnemy>();

        _enemyFSM._possibleStates.Add(EnemyFSMStates.Idle, new IdleState().SetUp(_enemyFSM).SetAvatar(this));
        _enemyFSM._possibleStates.Add(EnemyFSMStates.Move, new MoveState().SetUp(_enemyFSM).SetAvatar(this));
        _enemyFSM._possibleStates.Add(EnemyFSMStates.Attack, new AttackState().SetUp(_enemyFSM).SetAvatar(this));
        _enemyFSM._possibleStates.Add(EnemyFSMStates.Die, new DieState().SetUp(_enemyFSM).SetAvatar(this));

        _enemyFSM.ChangeState(EnemyFSMStates.Idle);
    }
    private void Start()
    {
        EnemyManager.Instance?.RegisterEnemy(this);
    }



    // Update is called once per frame
    void Update()
    {

        _enemyFSM.OnExecute();

        if (Input.GetKeyDown(KeyCode.C))
        {
            _enemyFSM.ChangeState(EnemyFSMStates.Move);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable<float> damageable = other.GetComponent<IDamageable<float>>();

        if (damageable != null)
        {
            damageable.TakeDamage(data.damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        IDamageable<float> damageable = other.GetComponent<IDamageable<float>>();

        if (damageable != null)
        {
            float timer = 5;

            timer -= Time.deltaTime;
            //Debug.Log(timer);
            if (timer <= 0)
            {
                timer = 5;
                damageable.TakeDamage(data.damage);
                Debug.Log("StartDamage");
            }
        }
    }
    public void Initialize(ObjectPool<BaseEnemy> pool, GoldResourseFactory goldFactory)
    {
        _myPool = pool;
        _goldFactory = goldFactory;
    }
    public virtual void Refresh()
    {
        _target = null;

    }

    #region Targeting
    public virtual void SearchForTarget()
    {
        currentTarget = EnemyTargetManager.Instance?.GetOptimalTarget(transform.position, targetingStrategy);
    }
    public void NotifyTargetLost(ITargetable lostTarget)
    {
        if (currentTarget == lostTarget)
        {
            currentTarget = null;
            // if (_enemyFSM._actualState() == EnemyFSMStates.Die) { }   

            _enemyFSM.ChangeState(EnemyFSMStates.Idle);
        }
    }
    #endregion
    #region Combat
    public virtual void PerformAttack()
    {
        if (currentTarget != null)//&& currentTarget.IsAlive)
        {
            IDamageable<float> damagable = currentTarget as IDamageable<float>;

        }
    }


    public override void Die()
    {
        _enemyFSM.ChangeState(EnemyFSMStates.Die);
        var gold = _goldFactory.Create();
        Vector3 pos = transform.position;
        pos.y = 1f;
        gold.transform.position = pos;
        ExperienceSystem.Instance.AddExperience(_experienceModidier);

        EnemyManager.Instance?.UnregisterEnemy(this);
        _myPool.Release(this);
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.Instance?.UnregisterEnemy(this);
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}