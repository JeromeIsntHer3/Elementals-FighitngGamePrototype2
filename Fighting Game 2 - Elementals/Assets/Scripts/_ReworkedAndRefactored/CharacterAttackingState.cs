using UnityEngine;

public class CharacterAttackingState : CharacterState
{
    public CharacterAttackingState(CharacterStateMachine context, CharacterStateFactory factory) : base(context, factory) { }

    bool landed;

    public override void EnterState()
    {
        _ctx.P_Animator.ClearRecovery();
        switch (_ctx.P_Character.CurrentAttack)
        {
            case 0:
                _ctx.P_Animator.SetAnimation(AnimationType.Attack1);
                break;
            case 1:
                _ctx.P_Animator.SetAnimation(AnimationType.Attack2);
                break;
            case 2:
                _ctx.P_Animator.SetAnimation(AnimationType.Attack3);
                break;
            case 3:
                _ctx.P_Animator.SetAnimation(AnimationType.Ultimate);
                break; 
            case 4:
                _ctx.P_Animator.SetAnimation(AnimationType.JumpAttack);
                break;
            default:
                Debug.Log("Attack Missing");
                break;
        }
        _ctx.P_Animator.SetAttackDuration(_ctx.P_Character.CurrentAttack);
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
        if (_ctx.P_Character.IsTouchingGround())
        {
            _ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
            landed = true;
        }
    }

    public override void UpdateAnimation()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (landed)
        {
            _ctx.P_Animator.ClearAttackRecovery();
            _ctx.P_Animator.ClearRecovery();
            _ctx.P_Animator.SetAnimation(AnimationType.JumpEnd);
            SwitchState(_factory.Grounded());
            return;
        }

        if (_ctx.P_Animator.IsAttackCompleted())
        {
            if(_ctx.P_Character.CurrentAttack != 4)
            {
                SwitchState(_factory.Grounded());
            }
            else
            {
                SwitchState(_factory.Jumping());
            }
        }
    }
}