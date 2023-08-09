using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Ctx = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterInput : MonoBehaviour
{
    PlayerControls controls;
    Vector2 movement;

    public EventHandler<Vector2> OnMovement;
    public EventHandler<Vector2> OnMovementPerformed;
    public EventHandler<Vector2> OnMovementCanceled;
    public EventHandler OnAttack1;
    public EventHandler OnAttack2;
    public EventHandler OnAttack3;
    public EventHandler OnUltimate;
    public EventHandler OnRoll;
    public EventHandler OnOption;
    public EventHandler OnOptionCanceled;
    public EventHandler OnJump;
    public EventHandler OnLand;
    public EventHandler<bool> OnChangeFaceDirection;

    void Awake()
    {
        controls = new ();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controls.Player.Movement.performed += (Ctx obj) => 
        {
            OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
            OnMovementPerformed?.Invoke(this, obj.ReadValue<Vector2>());
        };
        controls.Player.Movement.canceled += (Ctx obj) =>
        {
            OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
            OnMovementCanceled?.Invoke(this, obj.ReadValue<Vector2>());
        };

        controls.Player.Attack1.performed += (Ctx obj) =>
        {
            OnAttack1?.Invoke(this, EventArgs.Empty);
        };
        controls.Player.Attack2.performed += (Ctx obj) =>
        {
            OnAttack2?.Invoke(this, EventArgs.Empty);
        };
        controls.Player.Attack3.performed += (Ctx obj) =>
        {
            OnAttack3?.Invoke(this, EventArgs.Empty);
        };
        controls.Player.Ultimate.performed += (Ctx obj) =>
        {
            OnUltimate?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Roll.performed += (Ctx obj) =>
        {
            OnRoll?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Jump.performed += (Ctx obj) =>
        {
            OnJump?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Option.performed += (Ctx obj) =>
        {
            OnOption?.Invoke(this, EventArgs.Empty);
        };
        controls.Player.Option.canceled += (Ctx obj) =>
        {
            OnOptionCanceled?.Invoke(this, EventArgs.Empty);
        };
    }

    void Update()
    {
        movement = controls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 CurrentMovementInput()
    {
        return movement;
    }
}