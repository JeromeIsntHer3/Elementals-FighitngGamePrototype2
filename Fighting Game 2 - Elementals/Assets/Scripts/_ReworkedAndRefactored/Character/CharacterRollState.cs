using UnityEngine;

public class CharacterRollState : CharacterState
{
    public CharacterRollState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        Vector2 dir = Ctx.P_Character.IsFacingLeft ? Vector2.left : Vector2.right;
        Ctx.P_Character.ObjectRigidbody.velocity = new Vector2(0, Ctx.P_Character.ObjectRigidbody.velocity.y);
        Ctx.P_Character.ObjectRigidbody.AddForce(dir * Ctx.P_Character.MovementData.DashForce, ForceMode2D.Impulse);
        Ctx.P_Animator.SetAnimation(AnimationType.Roll);
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

    }

    public override void UpdateAnimation()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.P_Animator.Recovered())
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubStates()
    {
        
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}