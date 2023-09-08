using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState GameState = GameState.Menu;

    #region Events

    public class CharacterInfoArgs : EventArgs
    {
        public CharacterInfo PlayerOneInfo;
        public CharacterInfo PlayerTwoInfo;
        public PlayerInput PlayerOneInput;
        public PlayerInput PlayerTwoInput;
    }

    public class OnSelectCharacterArgs : EventArgs
    {
        public PlayableCharacter Character;
        public int PlayerIndex;
    }
    public static EventHandler<OnSelectCharacterArgs> OnSelectCharacter;
    public static EventHandler<OnSelectCharacterArgs> OnConfirmCharacter;

    public static EventHandler OnToMenu;
    public static EventHandler OnEnterMenu;
    public static EventHandler OnToCharacterSelect;
    public static EventHandler OnEnterCharacterSelect;
    public static EventHandler<CharacterInfoArgs> OnToGame;
    public static EventHandler OnEnterGame;
    public static EventHandler<int> OnGamePause;
    public static EventHandler OnToSettings;
    public static EventHandler OnGameOver;

    #endregion

    #region Static Variables

    //Meter
    public static int StartingMeterCount = 2;
    public static int MaxMeterCount = 4;
    public static int MaxMeterValue = 100;
    public static int BaseMeterGainOnHit = 4;
    public static int BaseMeterGainOnBlockHit = 2;
    public static int BaseMeterGainOnEnemyHit = 8;
    public static int BaseMeterGainOnEnemyBlockHit = 1;
    public static float StartingMeterValue = 100;

    //Blocking
    public static int MaxBlockHits = 10;
    public static float BlockResetDuration = 4f;
    public static float BaseDamageReduction = 95f;
    public static float BaseDamageReductionPerLevel = 4f;

    //UI Animation
    public static float UIAnimationDuration = 1f;

    readonly int FrameRate = 60;

    //Game Data
    public static float HealthChangeAnimationDuration = .25f;
    public static float MeterBurnThresholdTime = .25f;
    public static float ProjectileLifetime = 5f;
    public static float OnHitShakeAnimationMultiplier = .5f;
    public static float OnHitShakeStrengthX = .15f;
    public static float OnHitShakeStrengthY = .01f;
    public static int OnHitVibrato = 15;

    //Input Variables
    public static string UIInput = "UI";
    public static string PlayerInput = "Player";

    #endregion

    readonly Dictionary<int, PlayerInput> playerInputOfPlayer = new();
    readonly Dictionary<int, PlayerInputProxy> playerProxyOfPlayer = new();
    readonly Dictionary<int, GameObject> playerGameObjectOfPlayer = new();
    readonly Dictionary<int, Vector2> playerSpawnPosition = new();

    [SerializeField] TextMeshProUGUI gameStateText;

    void Awake()
    {
        Application.targetFrameRate = FrameRate;
        Instance = this;
        SetGameState(GameState.Menu);

        for (int i = 0; i < 2; i++)
        {
            playerInputOfPlayer.Add(i, null);
            playerProxyOfPlayer.Add(i, null);
            playerGameObjectOfPlayer.Add(i, null);
            playerSpawnPosition.Add(i, Vector2.zero);
        }
    }

    void OnEnable()
    {
        OnToMenu += GoToMenu;
        OnToCharacterSelect += GoToCharacterSelect;
        OnToGame += OnGoToGame;
        OnGamePause += PauseGame;
    }

    void OnDisable()
    {
        OnToMenu -= GoToMenu;
        OnToCharacterSelect -= GoToCharacterSelect;
        OnToGame -= OnGoToGame;
        OnGamePause -= PauseGame;
    }

    void OnGoToGame(object sender, CharacterInfoArgs args)
    {
        //Spawn Characters
        SetupCharacter(args.PlayerOneInput, args.PlayerOneInfo.RelativeSpawn, args.PlayerOneInfo.GamePrefab);
        Vector3 playerTwoSpawn = new(-args.PlayerTwoInfo.RelativeSpawn.x, args.PlayerTwoInfo.RelativeSpawn.y);
        SetupCharacter(args.PlayerTwoInput, playerTwoSpawn, args.PlayerTwoInfo.GamePrefab);

        //Add Characters to Camera and Disable Input First
        CameraManager.Instance.SetTargetGroup(playerGameObjectOfPlayer[0].transform, playerGameObjectOfPlayer[1].transform);
        EnablePlayerInput(false);

        //Sub Events of Characters
        GameUI.Instance.SubscribeGameEvents(playerGameObjectOfPlayer[0].GetComponent<BaseCharacter>(),
            playerGameObjectOfPlayer[1].GetComponent<BaseCharacter>());
        GameUI.Instance.SetupIcons(args.PlayerOneInfo.CharacterIcon, args.PlayerTwoInfo.CharacterIcon);

        //Setup Enemies Between one another
        playerGameObjectOfPlayer[0].GetComponent<BaseCharacter>().SetupCharacter(playerGameObjectOfPlayer[1].GetComponent<BaseCharacter>());
        playerGameObjectOfPlayer[1].GetComponent<BaseCharacter>().SetupCharacter(playerGameObjectOfPlayer[0].GetComponent<BaseCharacter>());
        
        //Setup variables
        for (int i = 0; i < 2; i++)
        {
            var playerObject = playerGameObjectOfPlayer[i];

            playerObject.GetComponent<BaseCharacterHealth>().SetupHealth(100, 100);
            playerObject.GetComponent<BaseCharacterAttacks>().SetupMeter(50, 2);
        }
    }

    void GoToMenu(object sender, EventArgs args)
    {
        if (playerGameObjectOfPlayer[0] == null) return;
        CameraManager.Instance.ClearTargetGroup(playerGameObjectOfPlayer[0].transform, playerGameObjectOfPlayer[1].transform);
    }

    void GoToCharacterSelect(object sender, EventArgs args)
    {
        if (playerGameObjectOfPlayer[0] == null) return;
        CameraManager.Instance.ClearTargetGroup(playerGameObjectOfPlayer[0].transform, playerGameObjectOfPlayer[1].transform);
    }

    void GoToGame(object sender,EventArgs args)
    {

    }

    void PauseGame(object sender, int pausedBy)
    {

    }

    void SetupCharacter(PlayerInput input, Vector2 position, GameObject prefab)
    {
        Vector2 spawnPos = position;

        var player = Instantiate(prefab, spawnPos, Quaternion.identity);
        player.GetComponent<CharacterInput>().SetInput(input);

        playerGameObjectOfPlayer[input.playerIndex] = player;
        playerSpawnPosition[input.playerIndex] = spawnPos;
    }

    public void SetupPlayerInputAndProxies(int playerIndex, PlayerInput input)
    {
        playerInputOfPlayer[playerIndex] = input;
        playerProxyOfPlayer[playerIndex] = input.GetComponent<PlayerInputProxy>();
        playerProxyOfPlayer[playerIndex].SetupProxy(playerIndex);
    }

    public void ClearPlayerInputAndProxies(int playerIndex)
    {
        playerProxyOfPlayer[playerIndex].SetSelectedObject(null);

        StartCoroutine(Utils.DelayEndFrame(() =>
        {
            Destroy(playerInputOfPlayer[playerIndex].gameObject);
            playerInputOfPlayer[playerIndex] = null;
            playerProxyOfPlayer[playerIndex] = null;
        }));
    }

    public PlayerInput GetPlayerInput(int index)
    {
        return playerInputOfPlayer[index];
    }

    public PlayerInputProxy GetPlayerProxy(int index)
    {
        return playerProxyOfPlayer[index];
    }

    public void SwitchMapsTo(string mapName)
    {
        playerInputOfPlayer[0].SwitchCurrentActionMap(mapName);
        playerInputOfPlayer[1].SwitchCurrentActionMap(mapName);
    }

    public void SetSelectionStateOfPlayers(bool state)
    {
        playerProxyOfPlayer[0].SetSelectionState(state);
        playerProxyOfPlayer[1].SetSelectionState(state);
    }

    public void RemovePlayerGameObjects()
    {
        Destroy(playerGameObjectOfPlayer[0]);
        Destroy(playerGameObjectOfPlayer[1]);
    }

    public void SetGameState(GameState state)
    {
        GameState = state;
        gameStateText.text = state.ToString();
    }

    public void EnablePlayerInput(bool state)
    {
        playerInputOfPlayer[0].enabled = state;
        playerInputOfPlayer[1].enabled = state;
        if (state) SwitchMapsTo(PlayerInput);
    }

    public void SwapPlayerInputControlScheme(PlayerInput player, string schemeName, params InputDevice[] devices)
    {
        player.SwitchCurrentControlScheme(schemeName, devices);
    }

    public void ResetPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            playerGameObjectOfPlayer[i].transform.position = playerSpawnPosition[i];
            playerGameObjectOfPlayer[i].GetComponent<BaseCharacterAnimator>().SetDeathFalse();
            playerGameObjectOfPlayer[i].GetComponent<BaseCharacterAttacks>().SetupMeter(50, 2);
            playerGameObjectOfPlayer[i].GetComponent<BaseCharacterHealth>().SetupHealth(100, 100);
        }
    }

    public int GetPlayerIndexWinner()
    {
        if (playerGameObjectOfPlayer[0].GetComponent<BaseCharacterHealth>().CurrentHealth >
            playerGameObjectOfPlayer[1].GetComponent<BaseCharacterHealth>().CurrentHealth)
        {
            return 0;
        }
        if (playerGameObjectOfPlayer[1].GetComponent<BaseCharacterHealth>().CurrentHealth >
            playerGameObjectOfPlayer[0].GetComponent<BaseCharacterHealth>().CurrentHealth)
        {
            return 1;
        }
        return -1;
    }
}

public enum GameState
{
    Menu, CharacterSelect, Game, GameOver, Pause
}