using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class BaseEnemy : Destructible
{
    [Header("Enemy Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] public float attackRange = 2f;
    [SerializeField] public float attackCooldown = 1f;
    [SerializeField] public float searchInterval = 0.5f;
    [SerializeField] float walkSpeed;
    [SerializeField] float idleTime;
    //[SerializeField] public float detectionRadius;
    //[SerializeField] public LayerMask targetMask;
    //[SerializeField] public float _energy;

    [Header("Targeting")]
    [SerializeField] protected TargetingStrategy targetingStrategy = TargetingStrategy.Nearest;
    public  ITargetable currentTarget;
    [SerializeField] Transform[] waypoints;
    [SerializeField] int currentWaypoint;
    public float lastAttackTime;
    public float lastSearchTime;



    [Header("Components")]
    [SerializeField] private GoldResourseFactory _goldFactory;
    public NavMeshAgent agent;
    protected Animator animator;


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
        agent.speed = walkSpeed;

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
            damageable.TakeDamage(damage);
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
                damageable.TakeDamage(damage);
                Debug.Log("StartDamage");
            }
        }
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
        if (currentTarget != null )//&& currentTarget.IsAlive)
        {
            IDamageable<float> damagable = currentTarget as IDamageable<float>;

            //Debug.Log($"PerformAttack with {damage}");
        }
    }


    public override void Die()
    {
        Debug.Log($"Die {_currentHealth}");
        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        //_enemyFSM.ChangeState(EnemyFSMStates.Die);
        var gold = _goldFactory.Create();
        gold.transform.position = transform.position + Vector3.up;
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.Instance?.UnregisterEnemy(this);
    }
    #endregion

    // Energy logic - skip for now
    //public bool ReduceEnergy(float fatigue)
    //{
    //    _energy -= fatigue;
    //    if (_energy <= 0)
    //    {
    //        _energy = 0;
    //        return true;
    //    }
    //    return false;
    //}
    //public void RecoverEnergy()
    //{
    //    _energy = 100;
    //}
    //public bool MoveTo(int index)
    //{
    //    var dirVector = waypoints[index].position - transform.position;
    //    transform.position += dirVector.normalized * Time.deltaTime * walkSpeed;
    //    if (dirVector.magnitude < 0.2f)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    //public void SetTarget(GameObject target)
    //{
    //    _target = target;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}