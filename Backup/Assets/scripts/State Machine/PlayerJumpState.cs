using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState() {
        Debug.Log("Jump State");
        if (_ctx.Jumped && _ctx.Grounded || _ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount))
        {
            HandleJump();
        }
    }
    public override void UpdateState() {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() {
        if (_ctx.Grounded)
        {
            SwitchState(_factory.Grounded());
        }
    }
    public override void InitalizeSubState() { }

    void HandleJump()
    {
        if (_ctx.Jumped && /*controller.isGrounded*/ _ctx.Grounded || _ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount))
        {
            Vector3 movement = _ctx.CurrentMovement;  // Get current movement
            movement.y += Mathf.Sqrt(_ctx.JumpMaxHeight * -1.2f * _ctx.GravityValue);
            //movement.y = Mathf.Clamp(_ctx.IntiaialJumpVelocity / _ctx.PlayerWeight
            //    , -_ctx.JumpMaxSpeed* _ctx.IntiaialJumpVelocity, _ctx.JumpMaxSpeed);
            _ctx.CurrentMovement = movement; // Apply updated movement
            _ctx.ANIMATOR.Play(_ctx.PLAYER_JUMP);

            _ctx.JumpCount++;
            _ctx.Jumped = false;
        }
    }
}
