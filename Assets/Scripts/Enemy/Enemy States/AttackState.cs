using UnityEngine;

public class AttackState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    public override void OnEnter()
    {
        Debug.Log($"Debug Enter a AttackState");
        avatar.agent.isStopped = true;
    }

    public override void OnExecute()
    {
        if (avatar.currentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
            return;
        }

        float distanceToTarget = Vector3.Distance(avatar.transform.position, avatar.currentTarget.GetPos());
        if (distanceToTarget > avatar.attackRange)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Move);
            return;
        }
        // Turning towards the target
        Vector3 lookDirection = (avatar.currentTarget.GetPos() - avatar.transform.position).normalized;
        lookDirection.y = 0;
        avatar.transform.rotation = Quaternion.LookRotation(lookDirection);

        // Attack with cooldown
        if (Time.time - avatar.lastAttackTime >= avatar.attackCooldown)
        {
            
            avatar.lastAttackTime = Time.time;
            avatar.PerformAttack();
        }
    }
    public override void OnExit()
    {
        
    }
}
