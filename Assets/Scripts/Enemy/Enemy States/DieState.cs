public class DieState : EnemyState<EnemyFSMStates, MVC_Enemy>
{
    public override void OnEnter()
    {
        avatar.Model.StopMovement();
    }
    public override void OnExecute() { }
    public override void OnExit() { }
}

