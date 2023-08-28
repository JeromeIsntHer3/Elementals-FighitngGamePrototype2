using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    void SpawnPlayer()
    {
        playerInputManager.JoinPlayer(0, default, "Keyboard");
        playerInputManager.JoinPlayer(1, default, "Controller");
    }
}