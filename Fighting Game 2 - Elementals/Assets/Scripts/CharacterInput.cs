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

    #endregion

    void Awake()
    {
        character = GetComponent<BaseCharacter>();
        playerInput = GetComponent<PlayerInput>();
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
    }

    void Start()
    {
        movementAction.performed += (Ctx obj) => 
        {
            character.OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
            character.OnMovementPerformed?.Invoke(this, obj.ReadValue<Vector2>());
        };
        movementAction.canceled += (Ctx obj) =>
        {
            character.OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
            character.OnMovementCanceled?.Invoke(this, obj.ReadValue<Vector2>());
        };

        attack1Action.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnAttack1?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack1));
        };
        attack2Action.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnAttack2?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack2));
        };
        attack3Action.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnAttack3?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack3));
        };
        ultimateAction.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnUltimate?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Ultimate));
        };

        rollAction.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnRoll?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Roll));
        };

        jumpAction.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnJump?.Invoke(this, EventArgs.Empty);
        };

        optionAction.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnOption?.Invoke(this, EventArgs.Empty);
        };

        optionAction.canceled += (Ctx obj) =>
        {
            character.OnOptionCanceled?.Invoke(this, EventArgs.Empty);
        };

        enhanceAction.performed += (Ctx obj) =>
        {
            character.OnTryEnhance?.Invoke(this, EventArgs.Empty);
        };
        cancelAction.performed += (Ctx obj) =>
        {
            character.OnTryCancel?.Invoke(this, EventArgs.Empty);
        };

        blockAction.performed += (Ctx obj) =>
        {
            if (!character.IsGrounded) return;
            character.OnBlock?.Invoke(this, EventArgs.Empty);
        };


        blockAction.canceled += (Ctx obj) =>
        {
            character.OnBlockCanceled?.Invoke(this, EventArgs.Empty);
        };
    }

    void Update()
    {
        movement = movementAction.ReadValue<Vector2>();
    }

    public Vector2 CurrentMovementInput()
    {
        return movement;
    }

    public void TriggerBlock()
    {
        if (!character.Recovered() || !character.IsGrounded) return;
        character.OnBlock?.Invoke(this, EventArgs.Empty);
    }

    public void CancelBlock()
    {
        character.OnBlockCanceled?.Invoke(this, EventArgs.Empty);
    }

    public void TriggerAttack1()
    {
        if (!character.Recovered()) return;
        character.OnAttack1?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack1));
    }

    public void TriggerAttack2()
    {
        if (!character.Recovered()) return;
        character.OnAttack2?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack2));
    }

    public void TriggerAttack3()
    {
        if (!character.Recovered()) return;
        character.OnAttack3?.Invoke(this, EventArgs.Empty);
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack3));
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CharacterInput))]
public class CharacterInputInspector : Editor
{
    SerializedProperty onBlockEvent;

    void OnEnable()
    {
        //onBlockEvent = serializedObject.FindProperty(nameof());
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterInput input = (CharacterInput)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Perform Block"))
        {
            input.TriggerBlock();
        }

        if (GUILayout.Button("Cancel Block"))
        {
            input.CancelBlock();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Perform Attack 1"))
        {
            input.TriggerAttack1();
        }

        if (GUILayout.Button("Perform Attack 2"))
        {
            input.TriggerAttack2();
        }

        if (GUILayout.Button("Perform Attack 3"))
        {
            input.TriggerAttack3();
        }

        GUILayout.EndHorizontal();
    }
}

#endif