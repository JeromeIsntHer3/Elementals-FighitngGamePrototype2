using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterJumpingState : CharacterState
{
    public CharacterJumpingState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    Rigidbody2D rb;
    CharacterMovementSO data;
    bool landed = false;
    bool started = false;

    public override void EnterState()
    {
        rb = _ctx.P_Character.Rb;
        data = _ctx.P_Character.MovementData;
        HandleJump();
    }

    public override void ExitState()
    {
        landed = false;
    }

    public override void FrameUpdate()
    {
        CheckSwitchStates();
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override AnimationType UpdateAnimation()
    {
        if (!started) 
        {
            started = true;
            return AnimationType.JumpStart;
        }
        if (landed)
        {
            return AnimationType.JumpEnd;
        }

        if (rb.velocity.y > 3f) return AnimationType.JumpRising;
        if (rb.velocity.y > 1f) return AnimationType.JumpPeak;
        return rb.velocity.y < -1f ? AnimationType.JumpFalling : AnimationType.JumpRising;
    }

    public override void CheckSwitchStates()
    {
        if (landed) SwitchState(_factory.Grounded());
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (_ctx.P_Character.IsTouchingGround())
        {
            landed = true;
        }
    }

    void HandleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * data.JumpForce, ForceMode2D.Impulse);
        rb.AddForce(_ctx.P_Character.Movement * data.JumpForce / 2, ForceMode2D.Impulse);
        _ctx.P_Character.Jumps -= 1;
    }
}