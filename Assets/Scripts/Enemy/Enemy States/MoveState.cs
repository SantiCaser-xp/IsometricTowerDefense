using UnityEngine;

public class MoveState : EnemyState<EnemyFSMStates, BaseEnemy>
{

    public override void OnEnter()
    {
        if (avatar.agent == null || !avatar.agent.enabled || !avatar.agent.isOnNavMesh)
            return;
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
        if (distanceToTarget <= avatar.data.attackRange)
        {
            avatar.agent.isStopped = true;
            enemyFSM.ChangeState(EnemyFSMStates.Attack);
        }
        else
        {
            avatar.agent.isStopped = false;
            avatar.agent.SetDestination(avatar.currentTarget.GetPos());
        }


    }
    public override void OnExit()
    {

    }
}
