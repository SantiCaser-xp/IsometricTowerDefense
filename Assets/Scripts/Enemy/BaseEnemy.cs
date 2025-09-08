using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, I_TestDamageable
{
    //Santino: Just for testing purposes, sorry if forgot to delete
    public void TakeDamage(float damageAmount)
    {
        TakeDamage(damageAmount);
    }


    [Header("Enemy Stats")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] public float attackRange = 2f;
    [SerializeField] public float attackCooldown = 1f;
    [SerializeField] public float searchInterval = 0.5f;
    [SerializeField] float walkSpeed;
    [SerializeField] float idleTime;
    [SerializeField] public float detectionRadius;
    [SerializeField] public LayerMask targetMask;
    [SerializeField] public float _energy;

    [Header("Targeting")]
    [SerializeField] protected TargetingStrategy targetingStrategy = TargetingStrategy.Nearest;
    public  ITargetable currentTarget;
    [SerializeField] Transform[] waypoints;
    [SerializeField] int currentWaypoint;
    public float lastAttackTime;
    public float lastSearchTime;



    [Header("Components")]
    public NavMeshAgent agent;
    protected Animator animator;

    public event Action<I_TestDamageable> OnDeath;


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
        animator = GetComponent<Animator>();

        EnemyManager.Instance?.RegisterEnemy(this);

        _enemyFSM = new EnemyFSM<EnemyFSMStates, BaseEnemy>();

        _enemyFSM._possibleStates.Add(EnemyFSMStates.Idle, new IdleState().SetUp(_enemyFSM).SetAvatar(this));
        _enemyFSM._possibleStates.Add(EnemyFSMStates.Move, new MoveState().SetUp(_enemyFSM).SetAvatar(this));
        _enemyFSM._possibleStates.Add(EnemyFSMStates.Attack, new AttackState().SetUp(_enemyFSM).SetAvatar(this));
        _enemyFSM._possibleStates.Add(EnemyFSMStates.Die, new DieState().SetUp(_enemyFSM).SetAvatar(this));

        _enemyFSM.ChangeState(EnemyFSMStates.Idle);
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
        
        if (currentTarget != null && currentTarget.IsAlive)
        {
            currentTarget.TakeDamage(damage);
            Debug.Log($"PerformAttack with {damage}");
        }
    }
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }




    public virtual void Die()
    {
        _enemyFSM.ChangeState(EnemyFSMStates.Die);

        OnDeath?.Invoke(this);
        Destroy(gameObject, 2f);

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

    //void I_TestDamageable.TakeDamage(int damageAmount)
    //{
    //    TakeDamage(damageAmount);
    //}
}
