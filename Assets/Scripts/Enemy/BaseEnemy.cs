using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public abstract class BaseEnemy : Destructible
{
    [Header("Enemy Stats")]
    [SerializeField] protected float _experienceModidier = 3f;

    [Header("Targeting")]
    [SerializeField] protected TargetingStrategy _targetingStrategy = TargetingStrategy.Nearest;
    [SerializeField] protected string _cState;
    [SerializeField] protected Transform[] _waypoints;
    protected int _currentWaypoint;
    protected ITargetable _currentTarget;
    public ITargetable CurrentTarget => _currentTarget;

    [Header("Components")]
    [SerializeField] protected GoldResourseFactory _goldFactory;
    protected NavMeshAgent _agent;

    public NavMeshAgent Agent => _agent;
    public Animator animator;
    protected ObjectPool<BaseEnemy> _myPool;
    [SerializeField] protected EnemyData _data;
    public EnemyData Data => _data;

    protected EnemyFSM<EnemyFSMStates, BaseEnemy> _enemyFSM;

    public int MaxWaypoints => _waypoints.Length;

    public static event System.Action OnEnemyKilled;


    private void Awake()
    {
        _currentHealth = _maxHealth;
        _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        _enemyFSM = new EnemyFSM<EnemyFSMStates, BaseEnemy>();

        _enemyFSM.ChangeState(EnemyFSMStates.Idle);
    }
    private void Start()
    {

    }

    void Update()
    {
        _enemyFSM.OnExecute();

    }

    public void Initialize(ObjectPool<BaseEnemy> pool, GoldResourseFactory goldFactory)
    {
        _myPool = pool;
        _goldFactory = goldFactory;
    }
    public virtual void Refresh()
    {
        _currentTarget = null;
        _enemyFSM.ChangeState(EnemyFSMStates.Idle);
    }

    #region Targeting
    public virtual void SearchForTarget()
    {
        _currentTarget = EnemyTargetManager.Instance?.GetOptimalTarget(transform.position, _targetingStrategy);
    }
    public void NotifyTargetLost(ITargetable lostTarget)
    {
        if (_currentTarget == lostTarget)
        {
            _currentTarget = null;

            _enemyFSM.ChangeState(EnemyFSMStates.Idle);
        }
    }
    #endregion
    #region Combat
    public virtual void PerformAttack()
    {
        if (_currentTarget != null)
        {
            IDamageable<float> damageable = _currentTarget as IDamageable<float>;
            if (damageable != null)
            {
                animator.SetTrigger("OnAttack");
                damageable.TakeDamage(_data.damage);
            }
        }

    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        animator.SetTrigger("OnHit");
        Debug.Log("Take override hit");
    }
    public override void Die()
    {
        animator.SetTrigger("OnDeath");
        _enemyFSM.ChangeState(EnemyFSMStates.Die);
        var gold = _goldFactory.Create();
        Vector3 pos = transform.position;
        pos.y = 1f;
        gold.transform.position = pos;
        ExperienceSystem.Instance.AddExperience(_experienceModidier);

        OnEnemyKilled?.Invoke();

        //EnemyManager.Instance?.UnregisterEnemy(this);
        _myPool.Release(this);
    }

    protected virtual void OnDestroy()
    {
        //EnemyManager.Instance?.UnregisterEnemy(this);
    }
    #endregion

    public void NavMeshAgentState(bool value)
    {
        Agent.isStopped = value;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _data.attackRange);
    }
}