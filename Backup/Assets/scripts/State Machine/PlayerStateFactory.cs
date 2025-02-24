//script used for convetsions
public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this); //_context,this
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this);
    }
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this);
    }
    public PlayerBaseState DoubleJump()
    {
        return new PlayerDoubleJumpState(_context, this);
    }
    public PlayerBaseState Grounded()
    {
        return new PlayerGroundState(_context, this);
    }
    public PlayerBaseState Attack1()
    {
        return new PlayerStateAttack1(_context, this);
    }
}