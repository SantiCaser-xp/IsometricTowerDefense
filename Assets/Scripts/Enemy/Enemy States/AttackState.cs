using UnityEngine;

public class AttackState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    float _timer;
    public override void OnEnter()
    {
        if (avatar.Agent == null || !avatar.Agent.enabled || !avatar.Agent.isOnNavMesh)
            return;
        avatar.NavMeshAgentState(true);
    }

    public override void OnExecute()
    {
        if (avatar.CurrentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
            return;
        }

        float distanceToTarget = (avatar.transform.position - avatar.CurrentTarget.GetPos()).sqrMagnitude;
        if (distanceToTarget > avatar.Data.attackRange * avatar.Data.attackRange)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Move);
            return;
        }

        // Turning towards the target
        Vector3 lookDirection = (avatar.CurrentTarget.GetPos() - avatar.transform.position).normalized;
        lookDirection.y = 0;
        avatar.transform.rotation = Quaternion.LookRotation(lookDirection);

        // Attack with cooldown
        _timer += Time.deltaTime;
        if (_timer >= avatar.Data.attackCooldown)
        {
            avatar.PerformAttack();
            _timer = 0;
        }
    }
    public override void OnExit()
    {

    }
}
