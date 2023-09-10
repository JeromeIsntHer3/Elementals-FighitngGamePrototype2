using UnityEngine;

public class CharacterBlockState : CharacterState
{
    public CharacterBlockState(CharacterStateMachine ctx, CharacterStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        _ctx.P_Animator.SetAnimation(AnimationType.DefendStart);
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
        _ctx.P_Animator.SetAnimation(AnimationType.DefendLoop);
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.P_Character.IsBlockPressed)
        {
            _ctx.P_Animator.SetAnimation(AnimationType.DefendEnd);
            SwitchState(_factory.Grounded());
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}