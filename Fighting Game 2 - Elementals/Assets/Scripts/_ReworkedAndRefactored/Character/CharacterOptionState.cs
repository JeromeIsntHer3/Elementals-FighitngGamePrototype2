using UnityEngine;

public class CharacterOptionState : CharacterState
{
    public CharacterOptionState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        if(Ctx.P_Character.HasOptionAnimation)
            Ctx.P_Animator.SetAnimation(AnimationType.CharacterOptionStart);
    }

    public override void ExitState()
    {
        if (Ctx.P_Character.HasOptionAnimation)
            Ctx.P_Animator.SetAnimation(AnimationType.CharacterOptionEnd);
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
        if (Ctx.P_Character.HasOptionAnimation)
            Ctx.P_Animator.SetAnimation(AnimationType.CharacterOptionLoop);
    }

    public override void CheckSwitchStates()
    {
        if(!Ctx.P_Character.IsOptionPressed)
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