using System.Collections;
using UnityEngine;

public class CharacterJumpingState : CharacterState
{
    public CharacterJumpingState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    Rigidbody2D rb;
    CharacterMovementSO data;
    bool landed = false;
    float canJumpTime;

    public override void EnterState()
    {
        rb = Ctx.P_Character.Rb;
        data = Ctx.P_Character.MovementData;
        
        canJumpTime = Time.time + Ctx.P_Character.DelayBetweenJumps;

        if (Ctx.P_PreviousState is not CharacterActionState)
        {
            Ctx.P_Animator.SetAnimation(AnimationType.JumpStart);
            Ctx.StartCoroutine(DelayJump());
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
        if (Ctx.P_Character.IsJumpPressed && Ctx.P_Character.CanJump())
        {
            canJumpTime = Time.time + Ctx.P_Character.DelayBetweenJumps;
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
            Ctx.P_Animator.SetAnimation(AnimationType.JumpRising);
        }
        else if (rb.velocity.y > 0f)
        {
            Ctx.P_Animator.SetAnimation(AnimationType.JumpPeak);
        }
        if (rb.velocity.y < -3f) Ctx.P_Animator.SetAnimation(AnimationType.JumpFalling);
    }

    public override void CheckSwitchStates()
    {
        if (landed)
        {
            SwitchState(Factory.Grounded());
            return;
        }
        if (Ctx.P_Character.IsAttackPressed)
        {
            Ctx.P_Character.CurrentAttack = 4;
            SwitchState(Factory.Action());
        }

        if(Ctx.P_PreviousState is CharacterAttackingState)
        {
            if (Ctx.P_Character.IsTouchingGround())
            {
                Ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
                landed = true;
            }
        }
    }

    public override void InitializeSubStates()
    {

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Ctx.P_Character.IsTouchingGround())
        {
            Ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
            landed = true;
        }
    }

    void HandleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * data.JumpForce, ForceMode2D.Impulse);
        rb.AddForce(Ctx.P_Character.Movement * data.JumpForce / 2, ForceMode2D.Impulse);
        Ctx.P_Character.JumpUsed();
    }

    void HandleMovement()
    {
        float targetSpeed = Ctx.P_Character.MovementData.PlayerSpeed * .65f * Ctx.P_Character.Movement.x;
        float speedDiff = targetSpeed - Ctx.P_Character.Rb.velocity.x;
        float movementRate = speedDiff * Ctx.P_Character.MovementData.AccelerationSpeed;
        Ctx.P_Character.Rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }

    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(Ctx.P_Animator.GetDuration(AnimationType.JumpStart));
        HandleJump();
    }
}