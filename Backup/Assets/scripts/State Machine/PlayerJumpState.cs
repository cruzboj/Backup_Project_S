using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    

    public override void EnterState()
    {

        _ctx.JumpBufferCounter = _ctx.JumpBufferTime;

        Debug.Log("Jump State");
        if (_ctx.Jumped && _ctx.Grounded || _ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount))
        {
            _ctx.ANIMATOR.Play(_ctx.PLAYER_JUMP);
            HandleJump();
        }
    }
    public override void UpdateState()
    {
        
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        if (_ctx.Jumped && (_ctx.JumpCount <= _ctx.MaxJumpCount))
        {
            SwitchState(_factory.DoubleJump());
        }
        else if (_ctx.Grounded)
        {
            //_ctx.ANIMATOR.Play(_ctx.PLAYER_JUMP_RECOIL);
            SwitchState(_factory.Grounded());
        }
    }
    public override void InitalizeSubState() { }


    void HandleJump()
    {
        if (_ctx.JumpBufferCounter >0f /*_ctx.Jumped*/ && _ctx.CoyoteTimeCounter > 0f /*&& _ctx.Grounded*/  /*|| _ctx.Jumped && (_ctx.JumpCount < _ctx.MaxJumpCount)*/)
        {
            _ctx.JumpBufferCounter = 0;
            //fix small jumps on walk
            if (_ctx.CurrentMovementInput.magnitude < _ctx.Threshold)
            {
                //new
                Vector3 movement = _ctx.CurrentMovement;
                movement.y = _ctx.InitialJumpVelocity * _ctx._jumpSpeed;
                _ctx.CurrentMovement = movement;
                _ctx.AppliedMovement = movement;

                //old
                //Vector3 movement = _ctx.CurrentMovement;  // Get current movement
                //movement.y += Mathf.Sqrt(_ctx.JumpMaxHeight * -2.2f * _ctx.GravityValue);
                //_ctx.CurrentMovement = movement; // Apply updated movement

                //VFX
                Object.Instantiate(_ctx.DoublejumpSmoke, _ctx.transform.position, Quaternion.Euler(90, 0, 0));

                _ctx.JumpCount++;
                _ctx.Jumped = false;
                _ctx.CoyoteTimeCounter = 0f;
            }
            //jumps on run
            else
            {
                //new
                Vector3 movement = _ctx.CurrentMovement;
                movement.y = _ctx.InitialJumpVelocity * _ctx._jumpSpeed;
                _ctx.CurrentMovement = movement;
                _ctx.AppliedMovement = movement;

                //old
                //Vector3 movement = _ctx.CurrentMovement;  // Get current movement
                //movement.y += Mathf.Sqrt(_ctx.JumpMaxHeight * -1.2f * _ctx.GravityValue);
                //_ctx.CurrentMovement = movement; // Apply updated movement

                //VFX
                Object.Instantiate(_ctx.DoublejumpSmoke, _ctx.transform.position, Quaternion.Euler(90, 0, 0));

                _ctx.JumpCount++;
                _ctx.Jumped = false;
                _ctx.CoyoteTimeCounter = 0f;
            }
        }
    }

    //private IEnumerator SpawnRecoilDelay(float delay)
    //{
        
    //    yield return new WaitForSeconds(delay);
    //    _ctx.ANIMATOR.Play(_ctx.PLAYER_JUMP_RECOIL);
    //}
}
