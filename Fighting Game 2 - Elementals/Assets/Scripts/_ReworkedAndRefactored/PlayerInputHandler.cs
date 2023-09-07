using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Ctx = UnityEngine.InputSystem.InputAction.CallbackContext;


//This version of the Player Inputs will handle events directly from the script. 
//Done to streamline the input system instead of having something messy and refers back
//and forth.
public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;

    //In-Game Inputs
    InputAction movementAction, rollAction, jumpAction, cancelAction, blockAction,
     attackOneAction, attackTwoAction, attackThreeAction, ultimateAction, optionAction, pauseGameAction;

    //UI Inputs


    #region Events

    //Event named without pressed will be used for both positive and negative actions
    //i.e. Pressing and Letting Go of the related key

    public EventHandler<Vector2> OnMovementPressed;
    public EventHandler OnRollPressed;
    public EventHandler<bool> OnJump;
    public EventHandler OnCancelPressed;
    public EventHandler<bool> OnBlock;
    public EventHandler<int> OnAttackPressed;
    public EventHandler<bool> OnOption;
    public EventHandler OnPauseGamePressed;

    #endregion


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        movementAction = playerInput.actions["Movement"];
        rollAction = playerInput.actions["Roll"];
        jumpAction = playerInput.actions["Jump"];
        cancelAction = playerInput.actions["Cancel"];
        blockAction = playerInput.actions["Block"];
        attackOneAction = playerInput.actions["Attack1"];
        attackTwoAction = playerInput.actions["Attack2"];
        attackThreeAction = playerInput.actions["Attack3"];
        ultimateAction = playerInput.actions["Ultimate"];
        optionAction = playerInput.actions["Option"];
        pauseGameAction = playerInput.actions["Pause"];
    }

    void OnEnable()
    {
        movementAction.performed += Movement;
        movementAction.canceled += Movement;
        rollAction.performed += RollPerformed;
        jumpAction.performed += Jump;
        jumpAction.canceled += Jump;
        cancelAction.performed += CancelPerformed;
        blockAction.started += Block;
        blockAction.canceled += Block;
        attackOneAction.performed += AttackOnePerformed;
        attackTwoAction.performed += AttackTwoPerformed;
        attackThreeAction.performed += AttackThreePerformed;
        ultimateAction.performed += AttackUltimatePerformed;
        optionAction.performed += Option;
        optionAction.canceled += Option;
        pauseGameAction.performed += PauseGamePerformed;
    }

    void OnDisable()
    {
        movementAction.performed -= Movement;
        movementAction.canceled -= Movement;
        rollAction.performed -= RollPerformed;
        jumpAction.performed -= Jump;
        jumpAction.canceled -= Jump;
        cancelAction.performed -= CancelPerformed;
        blockAction.started -= Block;
        blockAction.canceled -= Block;
        attackOneAction.performed -= AttackOnePerformed;
        attackTwoAction.performed -= AttackTwoPerformed;
        attackThreeAction.performed -= AttackThreePerformed;
        ultimateAction.performed -= AttackUltimatePerformed;
        optionAction.performed -= Option;
        optionAction.canceled -= Option;
        pauseGameAction.performed -= PauseGamePerformed;
    }

    void Movement(Ctx callbackContext)
    {
        OnMovementPressed?.Invoke(this, callbackContext.ReadValue<Vector2>());
    }

    void RollPerformed(Ctx callbackContext)
    {
        OnRollPressed?.Invoke(this, EventArgs.Empty);
    }

    void Jump(Ctx callbackContext)
    {
        OnJump?.Invoke(this, callbackContext.ReadValueAsButton());
    }

    void CancelPerformed(Ctx callbackContext)
    {
        OnCancelPressed?.Invoke(this, EventArgs.Empty);
    }

    void Block(Ctx callbackContext)
    {
        OnBlock?.Invoke(this, callbackContext.ReadValueAsButton());
    }

    void AttackOnePerformed(Ctx callbackContext)
    {
        OnAttackPressed?.Invoke(this, 0);
    }

    void AttackTwoPerformed(Ctx callbackContext)
    {
        OnAttackPressed?.Invoke(this, 1);
    }

    void AttackThreePerformed(Ctx callbackContext)
    {
        OnAttackPressed?.Invoke(this, 2);
    }

    void AttackUltimatePerformed(Ctx callbackContext)
    {
        OnAttackPressed?.Invoke(this, 3);
    }

    void Option(Ctx callbackContext)
    {
        OnOption?.Invoke(this, callbackContext.ReadValueAsButton());
    }

    void PauseGamePerformed(Ctx callbackContext)
    {
        OnPauseGamePressed?.Invoke(this, EventArgs.Empty);
    }
}