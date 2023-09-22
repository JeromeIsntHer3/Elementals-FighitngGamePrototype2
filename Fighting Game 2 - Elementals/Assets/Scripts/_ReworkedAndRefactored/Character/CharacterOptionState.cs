using UnityEngine;

public class CharacterOptionState : CharacterState
{
    public CharacterOptionState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
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

    }

    public override void CheckSwitchStates()
    {

    }

    public override void InitializeSubStates()
    {
        
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}