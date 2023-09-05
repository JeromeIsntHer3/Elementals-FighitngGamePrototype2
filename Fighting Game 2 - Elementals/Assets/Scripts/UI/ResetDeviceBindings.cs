using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetDeviceBindings : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputAction;
    [SerializeField] string targetControlScheme;

    public void ResetAllBindings()
    {
        foreach(InputActionMap map in _inputAction.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }

    public void ResetControlSchemeBinding()
    {
        foreach (InputActionMap map in _inputAction.actionMaps)
        {
            foreach(InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));
            }
        }
    }
}