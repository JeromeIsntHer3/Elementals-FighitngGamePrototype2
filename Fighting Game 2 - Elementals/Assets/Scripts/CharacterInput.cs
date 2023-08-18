using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Ctx = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterInput : MonoBehaviour
{
    PlayerControls controls;
    BaseCharacter character;
    Vector2 movement;

    void Awake()
    {
        controls = new ();
        character = GetComponent<BaseCharacter>();
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
            character.OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
            character.OnMovementPerformed?.Invoke(this, obj.ReadValue<Vector2>());
        };
        controls.Player.Movement.canceled += (Ctx obj) =>
        {
            character.OnMovement?.Invoke(this, obj.ReadValue<Vector2>());
            character.OnMovementCanceled?.Invoke(this, obj.ReadValue<Vector2>());
        };

        controls.Player.Attack1.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnAttack1?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack1));
        };
        controls.Player.Attack2.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnAttack2?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack2));
        };
        controls.Player.Attack3.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnAttack3?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Attack3));
        };
        controls.Player.Ultimate.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnUltimate?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Ultimate));
        };

        controls.Player.Roll.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnRoll?.Invoke(this, EventArgs.Empty);
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.Roll));
        };

        controls.Player.Jump.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnJump?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Option.performed += (Ctx obj) =>
        {
            if (!character.Recovered()) return;
            character.OnOption?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Option.canceled += (Ctx obj) =>
        {
            character.OnOptionCanceled?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Enhance.performed += (Ctx obj) =>
        {
            character.OnTryEnhance?.Invoke(this, EventArgs.Empty);
        };
        controls.Player.Cancel.performed += (Ctx obj) =>
        {
            character.OnTryCancel?.Invoke(this, EventArgs.Empty);
        };

        controls.Player.Block.performed += (Ctx obj) =>
        {
            if (!character.IsGrounded) return;
            character.OnBlock?.Invoke(this, EventArgs.Empty);
        };


        controls.Player.Block.canceled += (Ctx obj) =>
        {
            character.OnBlockCanceled?.Invoke(this, EventArgs.Empty);
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