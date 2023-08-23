using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
}