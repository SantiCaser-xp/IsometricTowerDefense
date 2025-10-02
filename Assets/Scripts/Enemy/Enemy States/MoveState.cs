using UnityEngine;

public class MoveState : EnemyState<EnemyFSMStates, BaseEnemy>
{

    public override void OnEnter()
    {
        if (avatar.Agent == null || !avatar.Agent.enabled || !avatar.Agent.isOnNavMesh)
            return;
        avatar.NavMeshAgentState(false);
    }

    public override void OnExecute()
    {
        if (avatar.CurrentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
            return;
        }
        float distanceToTarget = Vector3.Distance(avatar.transform.position, avatar.CurrentTarget.GetPos());
        if (distanceToTarget <= avatar.Data.attackRange)
        {
            avatar.NavMeshAgentState(true);
            enemyFSM.ChangeState(EnemyFSMStates.Attack);
        }
        else
        {
            avatar.NavMeshAgentState(false);
            avatar.Agent.SetDestination(avatar.CurrentTarget.GetPos());
        }


    }
    public override void OnExit()
    {

    }
}
