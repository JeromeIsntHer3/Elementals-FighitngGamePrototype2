using UnityEngine;

public class CharacterIdleState : CharacterState
{
    public CharacterIdleState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
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
        Ctx.P_Animator.SetAnimation(AnimationType.Idle);
    }

    public override void CheckSwitchStates()
    {
        if(Mathf.Abs(Ctx.P_Character.Movement.x) > 0)
        {
            SwitchState(Factory.Running());
        }
    }

    public override void InitializeSubStates()
    {

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}