using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState GameState = GameState.Menu;

    public class OnPlayersReadyArgs : EventArgs
    {
        public Vector2 PlayerOneSpawnPos;
        public Vector2 PlayerTwoSpawnPos;
        public GameObject PlayerOnePrefab;
        public GameObject PlayerTwoPrefab;
        public PlayerInput PlayerOneInput;
        public PlayerInput PlayerTwoInput;
    }
    public static EventHandler<OnPlayersReadyArgs> OnPlayersReady;

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

    readonly Dictionary<int, PlayerInput> playerInputOfPlayer = new();
    readonly Dictionary<int, PlayerInputProxy> playerProxyOfPlayer = new();

    [SerializeField] TextMeshProUGUI gameStateText;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = FrameRate;
        SetGameState(GameState.Menu);

        playerInputOfPlayer.Add(0, null);
        playerInputOfPlayer.Add(1, null);
        playerProxyOfPlayer.Add(0, null);
        playerProxyOfPlayer.Add(1, null);
    }

    void OnEnable()
    {
        OnPlayersReady += PlayerReady;
        MenuSceneManager.OnGoToCharacterSelect += PlayPressed;
        MenuSceneManager.OnGoToMenu += BackToMenu;
    }

    void OnDisable()
    {
        OnPlayersReady -= PlayerReady;
        MenuSceneManager.OnGoToCharacterSelect -= PlayPressed;
        MenuSceneManager.OnGoToMenu -= BackToMenu;
    }

    void PlayerReady(object sender, OnPlayersReadyArgs args)
    {
        var playerOneCharacter = Instantiate(args.PlayerOnePrefab, args.PlayerOneSpawnPos, Quaternion.identity);
        playerOneCharacter.GetComponent<CharacterInput>().SetInput(args.PlayerOneInput);
        //SwapPlayerInputControlScheme(args.PlayerOneInput, "Keyboard", Keyboard.current);

        Vector3 playerTwoSpawn = new(-args.PlayerTwoSpawnPos.x, args.PlayerTwoSpawnPos.y);

        var playerTwoCharacter = Instantiate(args.PlayerTwoPrefab, playerTwoSpawn, Quaternion.identity);
        playerTwoCharacter.GetComponent<CharacterInput>().SetInput(args.PlayerTwoInput);
        //SwapPlayerInputControlScheme(args.PlayerTwoInput, "Controller", Gamepad.current);

        CameraManager.Instance.SetTargetGroup(playerOneCharacter.transform, playerTwoCharacter.transform);
        SetGameState(GameState.Game);
        EnablePlayerInput(false);

        MenuSceneManager.OnGoToGame?.Invoke(this, EventArgs.Empty);
    }

    void PlayPressed(object sender, EventArgs args)
    {
        
    }

    void BackToMenu(object sender, EventArgs args)
    {
        
    }

    public void SetupPlayerInputAndProxies(int playerIndex, PlayerInput input)
    {
        playerInputOfPlayer[playerIndex] = input;
        playerProxyOfPlayer[playerIndex] = input.GetComponent<PlayerInputProxy>();
        playerProxyOfPlayer[playerIndex].SetupProxy(playerIndex);
    }

    public void ClearPlayerInputAndProxies(int playerIndex)
    {
        playerInputOfPlayer[playerIndex] = null;
        playerProxyOfPlayer[playerIndex] = null;
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

    public void SetGameState(GameState state)
    {
        GameState = state;
        gameStateText.text = state.ToString();
    }

    public void EnablePlayerInput(bool state)
    {
        playerInputOfPlayer[0].enabled = state;
        playerInputOfPlayer[1].enabled = state;
        if (state) SwitchMapsTo("Player");
    }

    public void SwapPlayerInputControlScheme(PlayerInput player, string schemeName, params InputDevice[] devices)
    {
        player.SwitchCurrentControlScheme(schemeName, devices);
    }
}

public enum GameState
{
    Menu, CharacterSelect, Game, GameOver, Pause
}