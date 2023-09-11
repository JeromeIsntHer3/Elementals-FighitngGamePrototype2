using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;

    [field: SerializeField] public PlayerInputManager PlayerInputMgr { get; set; }
    [field: SerializeField] public PlayerInput MenuInput { get; set; }
    [SerializeField] MultiplayerEventSystem multiplayerES;

    PlayerInput[] playerInputs = new PlayerInput[2];
    PlayerInputProxy[] playerInputProxies = new PlayerInputProxy[2];

    //Input Variables
    public static string UIScheme = "UI";
    public static string GameScheme = "Player";
    public static string UIBackAction = "Back";
    public static string UIChangeTabAction = "ChangeTab";


    void Awake()
    {
        Instance = this;
        MenuInput.onControlsChanged += OnControlsChanged;
    }

    void OnControlsChanged(PlayerInput obj)
    {
        Debug.Log("Changed to " + obj.currentControlScheme);
    }

    public void SetupPlayerInputAndProxies(int playerIndex, PlayerInput input)
    {
        playerInputs[playerIndex] = input;
        playerInputProxies[playerIndex] = input.GetComponent<PlayerInputProxy>();
        playerInputProxies[playerIndex].SetupProxy(playerIndex);
    }

    public void ClearPlayerInputAndProxies(int playerIndex)
    {
        playerInputProxies[playerIndex].SetSelectedObject(null);

        StartCoroutine(Utils.DelayEndFrame(() =>
        {
            Destroy(playerInputs[playerIndex].gameObject);
            playerInputs[playerIndex] = null;
            playerInputProxies[playerIndex] = null;
        }));
    }

    public PlayerInput GetPlayerInput(int index)
    {
        return playerInputs[index];
    }

    public PlayerInputProxy GetPlayerProxy(int index)
    {
        return playerInputProxies[index];
    }

    public PlayerInput SwitchPlayerMapTo(int index, string mapName)
    {
        playerInputs[index].SwitchCurrentActionMap(mapName);
        return playerInputs[index];
    }

    public PlayerInputProxy SetPlayerSelectionState(int index, bool state)
    {
        playerInputProxies[index].SetSelectionState(state);
        return playerInputProxies[index];
    }

    public void EnablePlayerInput(int index, bool state, string scheme)
    {
        playerInputs[index].enabled = state;
        if (state) SwitchPlayerMapTo(index,scheme);
    }

    public void SwapPlayerInputControlScheme(PlayerInput player, string schemeName, params InputDevice[] devices)
    {
        player.SwitchCurrentControlScheme(schemeName, devices);
    }

    public void SetMenuInputState(bool state)
    {
        MenuInput.enabled = state;
    }

    public void SetEventSystemSelection(GameObject root, GameObject obj)
    {
        multiplayerES.playerRoot = root;
        multiplayerES.SetSelectedGameObject(null);
        StartCoroutine(Utils.DelayEndFrame(() =>
        {
            multiplayerES.SetSelectedGameObject(obj);
        }));
    }
}