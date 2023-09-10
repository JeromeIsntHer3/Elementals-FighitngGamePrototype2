using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected CharacterStateMachine _ctx;
    protected CharacterStateFactory _factory;
    public CharacterState(CharacterStateMachine ctx, CharacterStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
    public abstract void CheckSwitchStates();
    public abstract void ExitState();
    public abstract void UpdateAnimation();
    public abstract void OnCollisionEnter2D(Collision2D collision);

    protected void SwitchState(CharacterState newState)
    {
        _ctx.P_PreviousState = this;
        ExitState();
        newState.EnterState();
        _ctx.P_CurrentState = newState;
        _ctx.currentStateText.text = _ctx.P_CurrentState.ToString();
    }

    protected void SetSuperState()
    {

    }

    protected void SetSubState()
    {

    }
}