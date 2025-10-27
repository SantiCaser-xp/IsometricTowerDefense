using UnityEngine;
public class MoveState : EnemyState<EnemyFSMStates, MVC_Enemy>
{
    public override void OnEnter()
    {

    }
    public override void OnExecute()
    {
        if (avatar.Model.CurrentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
            return;
        }

        float distanceToTarget = Vector3.Distance(avatar.Model.Position, avatar.Model.CurrentTarget.GetPos());

        if (distanceToTarget <= avatar.Model.Data.attackRange)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Attack);
        }
        else
        {
            avatar.Model.MoveTo(avatar.Model.CurrentTarget.GetPos());
        }
    }
    public override void OnExit() { }
}
