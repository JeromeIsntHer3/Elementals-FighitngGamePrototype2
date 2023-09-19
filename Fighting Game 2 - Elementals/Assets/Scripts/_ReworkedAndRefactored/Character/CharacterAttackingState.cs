using UnityEngine;

public class CharacterAttackingState : CharacterState
{
    public CharacterAttackingState(CharacterStateMachine context, CharacterStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        Ctx.P_Animator.ClearRecovery();
        switch (Ctx.P_Character.CurrentAttack)
        {
            case 0:
                Ctx.P_Animator.SetAnimation(AnimationType.Attack1);
                break;
            case 1:
                Ctx.P_Animator.SetAnimation(AnimationType.Attack2);
                break;
            case 2:
                Ctx.P_Animator.SetAnimation(AnimationType.Attack3);
                break;
            case 3:
                Ctx.P_Animator.SetAnimation(AnimationType.Ultimate);
                break; 
            case 4:
                Ctx.P_Animator.SetAnimation(AnimationType.JumpAttack);
                break;
            default:
                Debug.Log("Attack Missing");
                break;
        }
        Ctx.P_Animator.SetAttackDuration(Ctx.P_Character.CurrentAttack);
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

    public override void UpdateAnimation()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.P_Character.IsTouchingGround() && Ctx.P_PreviousState is CharacterJumpingState)
        {
            Ctx.P_Animator.ClearAttackRecovery();
            Ctx.P_Animator.ClearRecovery();
            Ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
            SwitchState(Factory.Grounded());
            return;
        }

        if (Ctx.P_Animator.IsAttackCompleted())
        {
            if (Ctx.P_Character.CurrentAttack != 4)
            {
                SwitchState(Factory.Grounded());
            }
            else
            {
                SwitchState(Factory.Jumping());
            }
        }

        //if (Ctx.P_Animator.IsAttackCompleted())
        //{
        //    if(Ctx.P_Character.CurrentAttack != 4)
        //    {
        //        SwitchState(Factory.Grounded());
        //    }
        //    else
        //    {
        //        SwitchState(Factory.Jumping());
        //    }
        //}
    }

    public override void InitializeSubStates()
    {
        
    }
}