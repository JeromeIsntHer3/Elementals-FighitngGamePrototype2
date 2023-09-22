using UnityEngine;

public class CharacterGroundedState : CharacterState
{
    public CharacterGroundedState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        Ctx.P_Character.ResetJumps();
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

    public override void UpdateAnimation()
    {
        if (Ctx.P_Character.Movement.x > 0)
        {
            Ctx.P_Animator.Sr.flipX = false;
        }
        else if (Ctx.P_Character.Movement.x < 0)
        {
            Ctx.P_Animator.Sr.flipX = true;
        }
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.P_Character.IsJumpPressed && Ctx.P_Character.CanJump())
        {
            SwitchState(Factory.Jumping());
        }

        if (Ctx.P_Character.IsBlockPressed || Ctx.P_Character.IsAttackPressed ||
            Ctx.P_Character.IsOptionPressed || Ctx.P_Character.IsRollPressed)
        {
            SwitchState(Factory.Action());
        }
    }

    public override void InitializeSubStates()
    {
        if(Mathf.Abs(Ctx.P_Character.Movement.x) > 0)
        {
            SetSubState(Factory.Running());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void HandleMovement()
    {
        float targetSpeed = Ctx.P_Character.MovementData.PlayerSpeed * Ctx.P_Character.Movement.x;
        float speedDiff = targetSpeed - Ctx.P_Character.Rb.velocity.x;
        float movementRate = speedDiff * Ctx.P_Character.MovementData.AccelerationSpeed;
        Ctx.P_Character.Rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }
}