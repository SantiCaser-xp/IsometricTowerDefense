using UnityEngine;

public class AttackState : EnemyState<EnemyFSMStates, MVC_Enemy>
{
    float _timer;
    public override void OnEnter()
    {
        avatar.Model.StopMovement();
        _timer = 0;
    }
    public override void OnExecute()
    {
        if (avatar.Model.CurrentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
            return;
        }

        float distanceToTarget = (avatar.Model.Position - avatar.Model.CurrentTarget.GetPos()).sqrMagnitude;

        if (distanceToTarget > avatar.Model.Data.attackRange * avatar.Model.Data.attackRange)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Move);
            return;
        }

        // Turning towards the target
        avatar.Model.RotateTowardTarget();

        // Attack with cooldown
        _timer += Time.deltaTime;
        if (_timer >= avatar.Model.Data.attackCooldown)
        {
            avatar.Model.PerformAttack();
            _timer = 0;
        }
    }
    public override void OnExit() { }
}
