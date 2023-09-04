using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Ctx = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterInput : MonoBehaviour
{
    PlayerInput playerInput;
    BaseCharacter character;
    Vector2 movement;
    int playerIndex;

    public int PlayerIndex {  get { return playerIndex; } }

    #region InputActions

    InputAction movementAction;
    InputAction rollAction;
    InputAction jumpAction;
    InputAction cancelAction;
    InputAction enhanceAction;
    InputAction blockAction;
    InputAction attack1Action;
    InputAction attack2Action;
    InputAction attack3Action;
    InputAction ultimateAction;
    InputAction optionAction;
    InputAction pauseGameAction;

    #endregion

    public PlayerInput SetInput(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        playerIndex = playerInput.playerIndex;

        character = GetComponent<BaseCharacter>();
        movementAction = playerInput.actions["Movement"];
        rollAction = playerInput.actions["Roll"];
        jumpAction = playerInput.actions["Jump"];
        cancelAction = playerInput.actions["Cancel"];
        enhanceAction = playerInput.actions["Enhance"];
        blockAction = playerInput.actions["Block"];
        attack1Action = playerInput.actions["Attack1"];
        attack2Action = playerInput.actions["Attack2"];
        attack3Action = playerInput.actions["Attack3"];
        ultimateAction = playerInput.actions["Ultimate"];
        optionAction = playerInput.actions["Option"];
        pauseGameAction = playerInput.actions["Pause"];

        movementAction.performed += MovePerformed;
        movementAction.canceled += MoveCanceled;

        rollAction.performed += RollPerformed;
        jumpAction.performed += JumpPerformed;

        attack1Action.performed += AttackOnePerformed;
        attack2Action.performed += AttackTwoPerformed;
        attack3Action.performed += AttackThreePerformed;
        ultimateAction.performed += UltimatePerformed;

        optionAction.performed += OptionPerformed;
        optionAction.canceled += OptionCanceled;

        enhanceAction.performed += EnhancePerformed;
        cancelAction.performed += CancelPerformed;

        blockAction.performed += BlockPerformed;
        blockAction.canceled += BlockCanceled;

        pauseGameAction.performed += PauseGamePerformed;

        return playerInput;
    }

    void OnDisable()
    {
        movementAction.performed -= MovePerformed;
        movementAction.canceled -= MoveCanceled;

        rollAction.performed -= RollPerformed;
        jumpAction.performed -= JumpPerformed;

        attack1Action.performed -= AttackOnePerformed;
        attack2Action.performed -= AttackTwoPerformed;
        attack3Action.performed -= AttackThreePerformed;
        ultimateAction.performed -= UltimatePerformed;

        optionAction.performed -= OptionPerformed;
        optionAction.canceled -= OptionCanceled;

        enhanceAction.performed -= EnhancePerformed;
        cancelAction.performed -= CancelPerformed;

        blockAction.performed -= BlockPerformed;
        blockAction.canceled -= BlockCanceled;

        pauseGameAction.performed -= PauseGamePerformed;
    }

    void Update()
    {
        movement = movementAction.ReadValue<Vector2>();
    }

    void MovePerformed(Ctx obj)
    {
        character.OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
        character.OnMovementPerformed?.Invoke(this, obj.ReadValue<Vector2>());
    }

    void MoveCanceled(Ctx obj)
    {
        character.OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
        character.OnMovementCanceled?.Invoke(this, obj.ReadValue<Vector2>());
    }

    void AttackOnePerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnAttackOne?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack1));
    }

    void AttackTwoPerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnAttackTwo?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack2));
    }

    void AttackThreePerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnAttackThree?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack3));
    }

    void UltimatePerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnUltimate?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Ultimate));
    }

    void JumpPerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnJump?.Invoke(this, EventArgs.Empty);
    }

    void RollPerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnRoll?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Roll));
    }

    void OptionPerformed(Ctx obj)
    {
        if (!character.Recovered()) return;
        character.OnOption?.Invoke(this, EventArgs.Empty);
    }

    void OptionCanceled(Ctx obj)
    {
        character.OnOptionCanceled?.Invoke(this, EventArgs.Empty);
    }

    void EnhancePerformed(Ctx obj)
    {
        character.OnTryEnhance?.Invoke(this, EventArgs.Empty);
    }

    void CancelPerformed(Ctx obj)
    {
        character.OnTryCancel?.Invoke(this, EventArgs.Empty);
    }

    void BlockPerformed(Ctx obj)
    {
        if (!character.IsGrounded) return;
        character.OnBlockPerformed?.Invoke(this, EventArgs.Empty);
    }

    void BlockCanceled(Ctx obj)
    {
        character.OnBlockCanceled?.Invoke(this, EventArgs.Empty);
    }

    void PauseGamePerformed(Ctx obj)
    {
        if(GameManager.GameState == GameState.Pause) return;
        UIManager.OnGamePause?.Invoke(this, playerIndex);
    }

    public Vector2 CurrentMovementInput()
    {
        return movement;
    }
}