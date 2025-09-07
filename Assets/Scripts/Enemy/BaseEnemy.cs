using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] int currentWaypoint;

    [SerializeField] float walkSpeed;
    [SerializeField] float idleTime;
    [SerializeField] public float detectionRadius;
    [SerializeField] public LayerMask targetMask;
    [SerializeField] public float _energy;
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
    public bool ReduceEnergy(float fatigue)
    {
        _energy -= fatigue;
        if (_energy <= 0)
        {
            _energy = 0;
            return true;
        }
        return false;
    }
    public void RecoverEnergy()
    {
        _energy = 100;
    }
    public bool MoveTo(int index)
    {
        var dirVector = waypoints[index].position - transform.position;
        transform.position += dirVector.normalized * Time.deltaTime * walkSpeed;
        if (dirVector.magnitude < 0.2f)
        {
            return true;
        }
        return false;
    }
    public void SetTarget(GameObject target)
    {
        _target = target;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
