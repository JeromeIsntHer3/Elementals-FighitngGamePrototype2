using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundedState : CharacterState
{
    public CharacterGroundedState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        _ctx.P_Character.Jumps = 2;
    }

    public override void ExitState()
    {
        
    }

    public override void FrameUpdate()
    {
        CheckSwitchStates();
    }

    public override void PhysicsUpdate()
    {
        HandleMovement();
    }

    public override AnimationType UpdateAnimation()
    {
        if (_ctx.P_Character.Movement.x > 0)
        {
            _ctx.P_Animator.Sr.flipX = false;
        }
        else if(_ctx.P_Character.Movement.x < 0)
        {
            _ctx.P_Animator.Sr.flipX = true;
        }

        return _ctx.P_Character.Movement.x == 0 ? AnimationType.Idle : AnimationType.Run;
    }

    public override void CheckSwitchStates()
    {
        if(_ctx.P_Character.IsJumpPressed && _ctx.P_Character.Jumps > 0)
        {
            SwitchState(_factory.Jumping());
        }

        if (_ctx.P_Character.IsBlockPressed)
        {
            SwitchState(_factory.Blocking());
        }

        if (_ctx.P_Character.IsOptionPressed)
        {
            //SwitchState(_factory.)
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void HandleMovement()
    {
        float targetSpeed = _ctx.P_Character.MovementData.PlayerSpeed * _ctx.P_Character.Movement.x;
        float speedDiff = targetSpeed - _ctx.P_Character.Rb.velocity.x;
        float movementRate = speedDiff * _ctx.P_Character.MovementData.AccelerationSpeed;
        _ctx.P_Character.Rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }
}