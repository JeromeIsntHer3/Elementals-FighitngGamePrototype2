using UnityEngine;

public class CharacterBlockState : CharacterState
{
    public CharacterBlockState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
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

    public override AnimationType UpdateAnimation()
    {
        return AnimationType.DefendLoop;
    }

    public override void CheckSwitchStates()
    {

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}