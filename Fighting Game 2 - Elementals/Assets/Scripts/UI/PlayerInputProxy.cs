using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerInputProxy : MonoBehaviour
{
    [SerializeField] CharacterButtonUI ranger, knight, bladekeeper, mauler;
    [SerializeField] MultiplayerEventSystem es;
    [SerializeField] Canvas canvas;

    [SerializeField] MultiplayerEventSystem currentSystem;
    PlayerInput playerInput;
    public EventHandler<int> OnDeselect;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        playerInput.actions[GameInputManager.UIBackAction].performed += BackPressed;
    }

    void OnDisable()
    {
        playerInput.actions[GameInputManager.UIBackAction].performed -= BackPressed;
    }

    void BackPressed(InputAction.CallbackContext obj)
    {
        OnDeselect?.Invoke(this, playerInput.playerIndex);
    }

    public PlayerInputProxy SetupProxy(int index)
    {
        currentSystem = es;
        ranger.SetupButtonUI(PlayableCharacter.LeafRanger, index);
        knight.SetupButtonUI(PlayableCharacter.FireKnight, index);
        bladekeeper.SetupButtonUI(PlayableCharacter.MetalBladekeeper, index);
        mauler.SetupButtonUI(PlayableCharacter.CrystalMauler, index);

        if(index == 0)
        {
            currentSystem.firstSelectedGameObject = ranger.gameObject;
        }
        else
        {
            currentSystem.firstSelectedGameObject = knight.gameObject;
        }

        return this;
    }

    public void SetSelectionState(bool state)
    {
        canvas.gameObject.SetActive(state);
    }

    public void SetEventSystemState(bool state)
    {
        StartCoroutine(Utils.DelayEndFrame(() =>
        {
            currentSystem.gameObject.SetActive(state);
        }));
    }

    public void SetEventSystem(MultiplayerEventSystem system)
    {
        currentSystem = system;
    }

    public void SetDefaultEventSystem()
    {
        currentSystem = es;
    }

    public void SetSelectedObject(GameObject gameObject)
    {
        currentSystem.SetSelectedGameObject(null);
        StartCoroutine(Utils.DelayEndFrame(() =>
        {
            currentSystem.SetSelectedGameObject(gameObject);
        }));
    }

    public void SetRootObject(GameObject gameObject)
    {
        currentSystem.playerRoot = gameObject;
    }

    public GameObject Selected()
    {
        return currentSystem.currentSelectedGameObject;
    }
 }