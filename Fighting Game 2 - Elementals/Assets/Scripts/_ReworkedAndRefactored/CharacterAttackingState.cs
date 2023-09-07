using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackingState : CharacterState
{
    public CharacterAttackingState(CharacterStateMachine context, CharacterStateFactory factory) : base(context, factory) { }

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

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public override AnimationType UpdateAnimation()
    {
        return _ctx.P_Character.CurrentAttack switch
        {
            0 => AnimationType.Attack1,
            1 => AnimationType.Attack2,
            2 => AnimationType.Attack3,
            3 => AnimationType.Ultimate,
            _ => AnimationType.Idle,
        };
    }

    public override void CheckSwitchStates()
    {
       
    }
}