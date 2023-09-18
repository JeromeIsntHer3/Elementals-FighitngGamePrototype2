using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsUI : BaseMenuUI
{
    [SerializeField] SelectionUI[] selections;

    PlayerInput playerInput;
    int currentSelectedIndex;

    void OnEnable()
    {
        playerInput = GameInputManager.Instance.MenuInput;
        playerInput.actions[GameInputManager.UIBackAction].performed += BackPressed;
        playerInput.actions[GameInputManager.UIChangeTabAction].performed += ChangeTab;

        selections[0].OnSelect();
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions[GameInputManager.UIBackAction].performed -= BackPressed;
            playerInput.actions[GameInputManager.UIChangeTabAction].performed -= ChangeTab;
            selections[0].OnDeselect();
            selections[1].OnDeselect();
        }
    }

    void BackPressed(InputAction.CallbackContext obj)
    {
        if (GameManager.GameState != GameState.Settings) return;
        GameManager.OnToMenu?.Invoke(this, EventArgs.Empty);
    }

    void ChangeTab(InputAction.CallbackContext obj)
    {
        selections[currentSelectedIndex].OnDeselect();

        currentSelectedIndex += (int) obj.ReadValue<float>();

        if(currentSelectedIndex < 0)
        {
            currentSelectedIndex = selections.Length - 1;
        }
        else if(currentSelectedIndex > selections.Length - 1)
        {
            currentSelectedIndex = 0;
        }

        selections[currentSelectedIndex].OnSelect();
    }
}