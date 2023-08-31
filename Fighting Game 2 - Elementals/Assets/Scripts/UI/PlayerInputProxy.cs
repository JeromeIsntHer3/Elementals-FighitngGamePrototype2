using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerInputProxy : MonoBehaviour
{
    [SerializeField] CharacterButtonUI ranger, knight, bladekeeper, mauler;
    [SerializeField] MultiplayerEventSystem es;
    [SerializeField] Canvas canvas;

    PlayerInput playerInput;
    public EventHandler OnDeselect;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void CharacterSelectProxyUI_performed(InputAction.CallbackContext obj)
    {
        OnDeselect?.Invoke(this, EventArgs.Empty);
    }

    void OnEnable()
    {
        playerInput.actions["Back"].performed += CharacterSelectProxyUI_performed;
    }

    void OnDisable()
    {
        playerInput.actions["Back"].performed -= CharacterSelectProxyUI_performed;
    }

    public PlayerInputProxy SetupProxy(MenuSceneManager m, int index)
    {
        ranger.SetupButtonUI(m, m.Ranger, index);
        knight.SetupButtonUI(m, m.Knight, index);
        bladekeeper.SetupButtonUI(m,m.Bladekeeper, index);
        mauler.SetupButtonUI(m, m.Mauler, index);

        if(index == 0)
        {
            es.firstSelectedGameObject = ranger.gameObject;
        }
        else
        {
            es.firstSelectedGameObject = knight.gameObject;
        }

        return this;
    }

    public void SetSelectionState(bool state)
    {
        canvas.gameObject.SetActive(state);
    }
}