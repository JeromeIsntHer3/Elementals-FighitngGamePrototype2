using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsUI : BaseMenuUI
{
    [SerializeField] SelectionUI[] selections;

    PlayerInput playerInput;
    int show = 0;

    void OnEnable()
    {
        playerInput = GameInputManager.Instance.MenuInput;
        playerInput.actions[GameInputManager.UIBackAction].performed += BackPressed;
        playerInput.actions[GameInputManager.UIChangeTabAction].performed += ChangeTab;

        if(show > 0)GameInputManager.Instance.SetEventSystemSelection(gameObject, selections[0].gameObject);
        show++;
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions[GameInputManager.UIBackAction].performed -= BackPressed;
            playerInput.actions[GameInputManager.UIChangeTabAction].performed -= ChangeTab;
        }
    }

    void BackPressed(InputAction.CallbackContext obj)
    {
        GameManager.OnToMenu?.Invoke(this, EventArgs.Empty);
    }

    void ChangeTab(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<float>());
    }
}