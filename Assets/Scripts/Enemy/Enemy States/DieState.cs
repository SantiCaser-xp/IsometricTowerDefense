public class DieState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    public override void OnEnter()
    {
        avatar.NavMeshAgentState(true);

    }
    public override void OnExecute()
    {

    }
    public override void OnExit()
    {

    }
}

