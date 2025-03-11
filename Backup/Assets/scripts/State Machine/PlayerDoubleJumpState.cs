using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    public PlayerDoubleJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("DoubleJump State");
        if (_ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount))
        {
            _ctx.ANIMATOR.Play(_ctx.PLAYER_DOUBLE_JUMP);
            HandleJump();
        }
    }
    public override void UpdateState() {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_ctx.Grounded)
        {
            _ctx.ANIMATOR.Play(_ctx.PLAYER_JUMP_RECOIL);
            SwitchState(_factory.Grounded());
        }
    }
    public override void InitalizeSubState() { }

    void HandleJump()
    {
        if (_ctx.Jumped && /*controller.isGrounded*/ _ctx.Grounded || _ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount))
        {
            //fix small jumps on walk
            if (_ctx.CurrentMovementInput.magnitude < _ctx.Threshold)
            {

                Vector3 movement = _ctx.CurrentMovement;  // Get current movement
                movement.y += Mathf.Sqrt(_ctx.JumpMaxHeight * -2.2f * _ctx.GravityValue);
                _ctx.CurrentMovement = movement; // Apply updated movement


                Object.Instantiate(_ctx.DoublejumpSmoke, _ctx.transform.position, Quaternion.Euler(90, 0, 0));
                //_ctx._grounded =false;
                _ctx.JumpCount++;
                _ctx.Jumped = false;
            }
            //jumps on run
            else
            {
                Vector3 movement = _ctx.CurrentMovement;  // Get current movement
                movement.y += Mathf.Sqrt(_ctx.JumpMaxHeight * -1.2f * _ctx.GravityValue);
                _ctx.CurrentMovement = movement; // Apply updated movement


                Object.Instantiate(_ctx.DoublejumpSmoke, _ctx.transform.position, Quaternion.Euler(90, 0, 0));
                //_ctx._grounded = false;
                _ctx.JumpCount++;
                _ctx.Jumped = false;
            }
        }
    }
}
