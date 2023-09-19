using UnityEngine;

public class CharacterBlockState : CharacterState
{
    public CharacterBlockState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        Ctx.P_Animator.SetAnimation(AnimationType.DefendStart);
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
        Ctx.P_Animator.SetAnimation(AnimationType.DefendLoop);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.P_Character.IsBlockPressed)
        {
            Ctx.P_Animator.SetAnimation(AnimationType.DefendEnd);
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