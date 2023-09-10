using System.Collections;
using UnityEngine;

public class CharacterJumpingState : CharacterState
{
    public CharacterJumpingState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    Rigidbody2D rb;
    CharacterMovementSO data;
    bool landed = false;
    float canJumpTime;

    public override void EnterState()
    {
        rb = _ctx.P_Character.Rb;
        data = _ctx.P_Character.MovementData;
        
        canJumpTime = Time.time + _ctx.P_Character.DelayBetweenJumps;

        if (_ctx.P_PreviousState is not CharacterAttackingState)
        {
            _ctx.P_Animator.SetAnimation(AnimationType.JumpStart);
            _ctx.StartCoroutine(DelayJump());
        }
    }

    public override void ExitState()
    {
        landed = false;
    }

    public override void FrameUpdate()
    {
        CheckSwitchStates();

        if (canJumpTime > Time.time) return;
        if (_ctx.P_Character.IsJumpPressed && _ctx.P_Character.CanJump())
        {
            canJumpTime = Time.time + _ctx.P_Character.DelayBetweenJumps;
            HandleJump();
        }
    }

    public override void PhysicsUpdate()
    {
        HandleMovement();
    }

    public override void UpdateAnimation()
    {
        if (rb.velocity.y > 3f)
        {
            _ctx.P_Animator.SetAnimation(AnimationType.JumpRising);
        }
        else if (rb.velocity.y > 0f)
        {
            _ctx.P_Animator.SetAnimation(AnimationType.JumpPeak);
        }
        if (rb.velocity.y < -3f) _ctx.P_Animator.SetAnimation(AnimationType.JumpFalling);
    }

    public override void CheckSwitchStates()
    {
        if (landed) SwitchState(_factory.Grounded());
        if (_ctx.P_Character.IsAttackPressed)
        {
            _ctx.P_Character.CurrentAttack = 4;
            SwitchState(_factory.Attacking());
        }

        if(_ctx.P_PreviousState is CharacterAttackingState)
        {
            if (_ctx.P_Character.IsTouchingGround())
            {
                _ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
                landed = true;
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (_ctx.P_Character.IsTouchingGround())
        {
            _ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
            landed = true;
        }
    }

    void HandleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * data.JumpForce, ForceMode2D.Impulse);
        rb.AddForce(_ctx.P_Character.Movement * data.JumpForce / 2, ForceMode2D.Impulse);
        _ctx.P_Character.JumpUsed();
    }

    void HandleMovement()
    {
        float targetSpeed = _ctx.P_Character.MovementData.PlayerSpeed * .65f * _ctx.P_Character.Movement.x;
        float speedDiff = targetSpeed - _ctx.P_Character.Rb.velocity.x;
        float movementRate = speedDiff * _ctx.P_Character.MovementData.AccelerationSpeed;
        _ctx.P_Character.Rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }

    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(_ctx.P_Animator.GetDuration(AnimationType.JumpStart));
        HandleJump();
    }
}