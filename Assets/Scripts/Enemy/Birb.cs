public class Birb : MVC_Enemy
{

    protected override void InitializeFSM()
    {
        fsm = new EnemyFSM<EnemyFSMStates, MVC_Enemy>();

        fsm._possibleStates.Add(EnemyFSMStates.Idle, new IdleState().SetUp(fsm).SetAvatar(this));
        fsm._possibleStates.Add(EnemyFSMStates.Move, new MoveState().SetUp(fsm).SetAvatar(this));
        fsm._possibleStates.Add(EnemyFSMStates.Attack, new KamikazeAttackState().SetUp(fsm).SetAvatar(this));
        fsm._possibleStates.Add(EnemyFSMStates.Die, new DieState().SetUp(fsm).SetAvatar(this));
    }
}
