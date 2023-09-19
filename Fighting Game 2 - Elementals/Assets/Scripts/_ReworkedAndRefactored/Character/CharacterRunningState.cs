using UnityEngine;

public class CharacterRunningState : CharacterState
{
    public CharacterRunningState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {

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
        Ctx.P_Animator.SetAnimation(AnimationType.Run);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.P_Character.Movement.x == 0)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubStates()
    {

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}