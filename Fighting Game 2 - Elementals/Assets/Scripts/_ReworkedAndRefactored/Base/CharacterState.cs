using UnityEngine;

public abstract class CharacterState
{
    bool _isRootState = false;
    CharacterStateMachine _ctx;
    CharacterStateFactory _factory;
    CharacterState _currSuperState;
    CharacterState _currSubState;

    protected bool IsRootState { get { return _isRootState; } set { _isRootState = value; } }
    protected CharacterStateMachine Ctx { get { return _ctx; } }
    protected CharacterStateFactory Factory { get { return _factory; } }

    public CharacterState(CharacterStateMachine ctx, CharacterStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubStates();
    public abstract void ExitState();
    public abstract void UpdateAnimation();
    public abstract void OnCollisionEnter2D(Collision2D collision);

    public void FrameUpdateStates()
    {
        FrameUpdate();
        UpdateAnimation();
        _currSubState?.FrameUpdate();
        _currSubState?.UpdateAnimation();

        _ctx.currentSuperStateText.text = _ctx.P_CurrentState.ToString();
        if (_currSubState == null)
        {
            _ctx.currentSubStateText.text = "Empty";
        }
        else
        {
            _ctx.currentSubStateText.text = _currSubState.ToString();
        }
    }

    public void PhysicsUpdateStates()
    {
        PhysicsUpdate();
        _currSubState?.PhysicsUpdate();
    }

    protected void SwitchState(CharacterState newState)
    {
        if (_ctx.P_CurrentState.IsRootState)
        {
            _ctx.P_PreviousState = _ctx.P_CurrentState;
        }
        ExitState();
        if (newState.IsRootState)
        {
            _currSubState?.ExitState();
            _currSubState = null;
            _ctx.P_CurrentState = newState;
            _ctx.P_CurrentState.EnterState();
        }
        else if(_currSuperState != null)
        {
            _currSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(CharacterState newSuperState)
    {
        _currSuperState = newSuperState;
    }

    protected void SetSubState(CharacterState newSubState)
    {
        _currSubState = newSubState;
        _currSubState.EnterState();
        newSubState.SetSuperState(this);
    }
}