using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerInputProxy : MonoBehaviour
{
    [SerializeField] CharacterButtonUI ranger, knight, bladekeeper, mauler;
    [SerializeField] MultiplayerEventSystem es;
    [SerializeField] Canvas canvas;

    PlayerInput playerInput;
    public EventHandler<int> OnDeselect;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        playerInput.actions["Back"].performed += BackPressed;
    }

    void OnDisable()
    {
        playerInput.actions["Back"].performed -= BackPressed;
    }

    void BackPressed(InputAction.CallbackContext obj)
    {
        OnDeselect?.Invoke(this, playerInput.playerIndex);
    }

    public PlayerInputProxy SetupProxy(int index)
    {
        ranger.SetupButtonUI(PlayableCharacter.LeafRanger, index);
        knight.SetupButtonUI(PlayableCharacter.FireKnight, index);
        bladekeeper.SetupButtonUI(PlayableCharacter.MetalBladekeeper, index);
        mauler.SetupButtonUI(PlayableCharacter.CrystalMauler, index);

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

    public void SetEventSystemState(bool state)
    {
        es.gameObject.SetActive(state);
    }

    public void SetEventSystem(MultiplayerEventSystem system)
    {
        es = system;
    }

    public void SetSelectedObject(GameObject gameObject)
    {
        es.SetSelectedGameObject(gameObject);
    }

    public GameObject Selected()
    {
        return es.currentSelectedGameObject;
    }
 }