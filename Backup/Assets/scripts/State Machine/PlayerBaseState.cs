
public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    //converter to 2 varibles PlayerStateMachine() PlayerStateFactory() into 1 to pass it into states 
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();          //=0
    public abstract void UpdateState();         //=0
    public abstract void ExitState();           //=0
    public abstract void CheckSwitchStates();   //=0
    public abstract void InitalizeSubState();   //=0

    void UpdateStates() { }

    protected void SwitchState(PlayerBaseState newState) {
        //current State exit state
        ExitState();

        //new state enters state
        newState.EnterState();

        //switch current state of the context
        _ctx.CurrentState = newState;
    }

    protected void SetSuperState() { }

    protected void SetSubState() { }
}
