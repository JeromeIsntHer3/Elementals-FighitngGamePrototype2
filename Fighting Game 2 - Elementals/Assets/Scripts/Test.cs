using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        playerInput.onControlsChanged += PlayerInput_onControlsChanged;

        InputSystem.AddDevice<Gamepad>();

        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
    }

    private void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
    {
        Debug.Log(arg1 + " changed to " + arg2);
    }

    private void PlayerInput_onControlsChanged(PlayerInput obj)
    {
        Debug.Log(obj.currentControlScheme);
    }
}