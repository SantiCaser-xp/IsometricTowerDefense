using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MVC_Enemy : Destructible
{
    [Header("MVC links")]
    [SerializeField] private EnemyData _data;
    //[SerializeField] private float _maxHealth;
    [SerializeField] protected float _experienceModidier = 3f;

    [Header("Dependencies (View)")]
    [SerializeField] private ParticleSystem _particleDmg;
    [SerializeField] private AudioSource _soundDmg;

    [Header("Components")]
    [SerializeField] protected GoldResourseFactory _goldFactory;
    [SerializeField] protected ObjectPool<MVC_Enemy> _myPool;
    private NavMeshAgent _agent;
    private Animator _animator;

    [Header("Tutorial")]
    [SerializeField] bool _tutorialMode = false;

    public MVC_EnemyModel Model { get; private set; }
    private MVC_EnemyView _view;
    private EnemyFSM<EnemyFSMStates, MVC_Enemy> _fsm;

    public static event System.Action OnEnemyKilled;
    //public TargetType TargetType => TargetType.Tower;
    protected ITargetable _currentTarget;
    [SerializeField] protected TargetingStrategy _targetingStrategy = TargetingStrategy.Nearest;
    [SerializeField] protected string _cState;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        Model = new MVC_EnemyModel(_agent, transform, _data, _maxHealth);
        _view = new MVC_EnemyView(Model, _animator, _particleDmg, _soundDmg);

        _fsm = new EnemyFSM<EnemyFSMStates, MVC_Enemy>();

        _fsm._possibleStates.Add(EnemyFSMStates.Idle, new IdleState().SetUp(_fsm).SetAvatar(this));
        _fsm._possibleStates.Add(EnemyFSMStates.Move, new MoveState().SetUp(_fsm).SetAvatar(this));
        _fsm._possibleStates.Add(EnemyFSMStates.Attack, new AttackState().SetUp(_fsm).SetAvatar(this));
        _fsm._possibleStates.Add(EnemyFSMStates.Die, new DieState().SetUp(_fsm).SetAvatar(this));

        Model.OnDie += HandleDeathLogic;
    }

    private void Start()
    {
        EnemyManager.Instance?.RegisterEnemy(this);
        _fsm.ChangeState(EnemyFSMStates.Idle);
    }

    void Update()
    {
        _fsm?.OnExecute();
        //_cState = $"{_fsm._actualState}";// for debug
    }

    private void HandleDeathLogic()
    {
        _fsm.ChangeState(EnemyFSMStates.Die);
        var gold = _goldFactory.Create();
        Vector3 pos = transform.position;
        pos.y = 1f;
        gold.transform.position = pos;
        ExperienceSystem.Instance.AddExperience(_experienceModidier);

        if(_tutorialMode) EventManager.Trigger(EventType.KillFirstEnemy, EventType.KillFirstEnemy);
        EventManager.Trigger(EventType.OnEnemyKilled);
        OnEnemyKilled?.Invoke();

        EnemyManager.Instance?.UnregisterEnemy(this);
        _myPool.Release(this);
    }

    protected virtual void OnDestroy()
    {
        _view.CleanUp();
        Model.OnDie -= HandleDeathLogic;

        EnemyManager.Instance?.UnregisterEnemy(this);
    }

    public override void TakeDamage(float damage)
    {
        Model.TakeDamage(damage);
    }

    public Vector3 GetPos()
    {
        return Model.GetPos();
    }

    public void Subscribe(IObserver observer)
    {
        Model.Subscribe(observer);
    }

    public virtual void Unsubscribe(IObserver observer)
    {
        Model.Unsubscribe(observer);
    }

    public override void Die()
    {
        Model.Die();
    }

    public virtual void SearchForTarget()
    {
        _currentTarget = EnemyTargetManager.Instance?.GetOptimalTarget(transform.position, _targetingStrategy);
        Model.SetTarget(_currentTarget);
    }

    public void NotifyTargetLost(ITargetable lostTarget)
    {
        if (Model.CurrentTarget == lostTarget)
        {
            Model.SetTarget(null);

            _fsm.ChangeState(EnemyFSMStates.Idle);
        }
    }

    public void Initialize(ObjectPool<MVC_Enemy> pool, GoldResourseFactory goldFactory)
    {
        _myPool = pool;
        _goldFactory = goldFactory;
    }
    public virtual void Refresh()
    {
        Model.SetTarget(null);
        Model.ResetHealth();
        EnemyManager.Instance?.RegisterEnemy(this);
        _fsm.ChangeState(EnemyFSMStates.Idle);
    }
}