using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Meter
    public static int StartingMeterCount = 2;
    public static int MaxMeterCount = 4;
    public static int MaxMeterValue = 100;
    public static int BaseMeterGainOnEnemyHit = 10;
    public static int BaseMeterGainOnBlockHit = 2;
    public static int BaseMeterGainOnHit = 4;
    public static float StartingMeterValue = 100;

    //Blocking
    public static int MaxBlockHits = 10;
    public static float BlockResetDuration = 4f;
    public static float BaseDamageReduction = 95f;
    public static float BaseDamageReductionPerLevel = 4f;

    [SerializeField] int FrameRate;
    [SerializeField] BaseCharacter playerOne;
    [SerializeField] BaseCharacter playerTwo;

    [Header("Game Data")]
    public float HpAnimDuration;
    public float MeterBurnThresholdTime;
    public float ProjectileDuration;
    [Range(1,100)] public float HitShakeAnim;
    public float ShakeStrengthX;
    public float ShakeStrengthY;
    public int Vibrato;
    public Color RCColor;
    public Color ECColor;

    #region Getter And Setters

    public BaseCharacter PlayerOne { get { return playerOne; } }
    public BaseCharacter PlayerTwo { get { return playerTwo; } }

    #endregion

    PlayerInputManager playerInputManager;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = FrameRate;
        playerInputManager = GetComponent<PlayerInputManager>();
        //SpawnPlayer();
    }

    void SpawnPlayers()
    {
        var playerOne = playerInputManager.JoinPlayer(0, default, "Keyboard");

        //playerOne.GetComponent<BaseCharacter>().SetupCharacter();

        playerInputManager.JoinPlayer(1, default, "Controller");
    }
}