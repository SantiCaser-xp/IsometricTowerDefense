using UnityEngine;

public class IdleState : EnemyState<EnemyFSMStates, MVC_Enemy>
{
    float _timer;

    public override void OnEnter()
    {
        avatar.Model.StopMovement();
    }

    public override void OnExecute()
    {
        // Set target logic
        _timer += Time.deltaTime;
        if (_timer >= avatar.Model.Data.searchInterval)
        {
            avatar.SearchForTarget();
            _timer = 0;
        }

        if (avatar.Model.CurrentTarget != null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Move);
        }
    }
    public override void OnExit() { }
}