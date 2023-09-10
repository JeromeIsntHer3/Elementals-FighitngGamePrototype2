using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStunnedState : CharacterState
{
    public CharacterStunnedState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
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
        //return AnimationType.Hit;
    }

    public override void CheckSwitchStates()
    {

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
