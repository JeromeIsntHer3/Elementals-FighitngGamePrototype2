using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerUIManager : MonoBehaviour
{
    PlayerInputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        
    }

    public void SetupPlayer(PlayerInput input)
    {
        input.uiInputModule = MenuSceneManager.Instance.GetComponent<InputSystemUIInputModule>();
    }
}