using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void EnterState() {
        Debug.Log("Ground STATE");
    }
    public override void UpdateState() {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() {
        if (_ctx.Jumped && _ctx.Grounded || _ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount))
        {
            SwitchState(_factory.Jump());
        }
    }
    public override void InitalizeSubState() { }
}
