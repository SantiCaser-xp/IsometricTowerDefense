using UnityEngine;

public class IdleState : EnemyState<EnemyFSMStates, BaseEnemy>
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
        // Set target logic
        _timer += Time.deltaTime;
        if (_timer >= avatar.Data.searchInterval)
        {
            avatar.SearchForTarget();
            _timer = 0;
        }

        if (avatar.CurrentTarget != null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Move);
        }



    }
    public override void OnExit()
    {

    }
}