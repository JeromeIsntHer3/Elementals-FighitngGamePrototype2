using UnityEngine;

public class CharacterActionState : CharacterState
{
    public CharacterActionState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
        InitializeSubStates();
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
        if (Ctx.P_Character.IsAttackPressed)
        {
            SetSubState(Factory.Attacking());
        }

        if (Ctx.P_Character.IsBlockPressed)
        {
            SetSubState(Factory.Blocking());
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}