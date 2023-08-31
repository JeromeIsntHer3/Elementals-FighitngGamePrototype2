using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState = GameState.Menu;

    public class OnPlayersReadyArgs : EventArgs
    {
        public Vector2 PlayerOneSpawnPos;
        public Vector2 PlayerTwoSpawnPos;
        public GameObject PlayerOnePrefab;
        public GameObject PlayerTwoPrefab;

        public OnPlayersReadyArgs(Vector2 playerOneSpawnPos, Vector2 playerTwoSpawnPos, 
            GameObject pOnePrefab, GameObject pTwoPrefab)
        {
            PlayerOneSpawnPos = playerOneSpawnPos;
            PlayerTwoSpawnPos = playerTwoSpawnPos;
            PlayerOnePrefab = pOnePrefab;
            PlayerTwoPrefab = pTwoPrefab;
        }
    }
    public EventHandler<OnPlayersReadyArgs> OnPlayersReady;

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

    int FrameRate = 60;

    [Header("Game Data")]
    public float HpAnimDuration;
    public float MeterBurnThresholdTime;
    public float ProjectileDuration;
    [Range(1,100)] public float HitShakeAnim;
    public float ShakeStrengthX;
    public float ShakeStrengthY;
    public int Vibrato;

    Vector3 playerOneSpawnPosition;
    Vector3 playerTwoSpawnPosition;
    GameObject prefabOne;
    GameObject prefabTwo;
    PlayerInput playerOneInput;
    PlayerInput playerTwoInput;

    public BaseCharacter PlayerOne => playerOneInput.GetComponent<BaseCharacter>();
    public BaseCharacter PlayerTwo => playerTwoInput.GetComponent<BaseCharacter>();

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = FrameRate;
    }

    void OnEnable()
    {
        OnPlayersReady += PlayerReady;
    }

    void OnDisable()
    {
        OnPlayersReady -= PlayerReady;
    }

    void PlayerReady(object sender, OnPlayersReadyArgs args)
    {
        playerOneSpawnPosition = args.PlayerOneSpawnPos;
        playerTwoSpawnPosition = args.PlayerTwoSpawnPos;
        prefabOne = args.PlayerOnePrefab;
        prefabTwo = args.PlayerTwoPrefab;

        var playerOneCharacter = Instantiate(prefabOne, playerOneSpawnPosition, Quaternion.identity);
        playerOneInput = playerOneCharacter.GetComponent<PlayerInput>();
        playerOneInput.GetComponent<CharacterInput>().SetInput(playerOneInput);
        playerOneInput.SwitchCurrentControlScheme("Keyboard", Keyboard.current);

        var playerTwoCharacter = Instantiate(prefabTwo, playerTwoSpawnPosition * -1, Quaternion.identity);
        playerTwoInput = playerTwoCharacter.GetComponent<PlayerInput>();
        playerTwoInput.GetComponent<CharacterInput>().SetInput(playerTwoInput);
        playerTwoInput.SwitchCurrentControlScheme("Controller", Gamepad.current);

        CameraManager.Instance.SetTargetGroup(playerOneCharacter.transform, playerTwoCharacter.transform);
        CameraManager.Instance.SetGamCams();
        GameState = GameState.Game;
    }

    public float PlayerDistance()
    {
        return Vector2.Distance(playerOneInput.transform.position, playerTwoInput.transform.position);
    }
}

public enum GameState
{
    Menu, CharacterSelect, Game, GameOver, Pause
}