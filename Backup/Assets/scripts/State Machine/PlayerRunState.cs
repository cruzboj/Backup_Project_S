
public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext , PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void EnterState() { }
    public override void UpdateState() { }
    public override void ExitState() { }
    public override void CheckSwitchStates() { }
    public override void InitalizeSubState() { }
}
