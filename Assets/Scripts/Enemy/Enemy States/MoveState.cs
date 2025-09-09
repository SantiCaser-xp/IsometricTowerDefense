using UnityEngine;

public class MoveState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    private int _actualWaypoint = 0;
    private float _attackCoolDown;
    public override void OnEnter()
    {
       
        avatar.agent.isStopped = false;
    }

    public override void OnExecute()
    {
        if (avatar.currentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
            return;
        }
        float distanceToTarget = Vector3.Distance(avatar.transform.position, avatar.currentTarget.GetPos());
        if (distanceToTarget <= avatar.attackRange)
        {
            avatar.agent.isStopped = true;
            enemyFSM.ChangeState(EnemyFSMStates.Attack);
        }
        else
        {
            avatar.agent.isStopped = false;
            avatar.agent.SetDestination(avatar.currentTarget.GetPos());
        }



        // Energy logic - skip for now
        //_attackCoolDown -= Time.deltaTime;
        //if (avatar.ReduceEnergy(Time.deltaTime * 5))
        //{
        //    enemyFSM.ChangeState(EnemyFSMStates.Idle);
        //}

        //From Patrol to Chase


        //var chaseVector = avatar._target.transform.position - avatar.transform.position;
        //if (chaseVector.magnitude > 0.3f)
        //{
        //    avatar.transform.position += chaseVector.normalized * Time.deltaTime * 5;
        //}
        //else if (_attackCoolDown <= 0) 
        //{
        //    enemyFSM.ChangeState(EnemyFSMStates.Attack);
        //    _attackCoolDown = 2f;
        //}

        //if (avatar.MoveTo(_actualWaypoint))
        //{

        //    _actualWaypoint++;
        //    if (_actualWaypoint >= avatar.maxWaypoints)
        //    {
        //        _actualWaypoint = 0;
        //    }
        //}

    }
    public override void OnExit()
    {
        //avatar.SetTarget(null);
    }
}
