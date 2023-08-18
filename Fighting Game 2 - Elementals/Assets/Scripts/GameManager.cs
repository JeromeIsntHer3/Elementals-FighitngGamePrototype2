using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int FrameRate;
    [SerializeField] BaseCharacter playerOne;
    [SerializeField] BaseCharacter playerTwo;
    [SerializeField] float healthAnimDuration;
    [SerializeField] float meterBurnThreshold;
    [SerializeField] float projectileDuration;
    [SerializeField] float hitShakeAnim;
    public float DistanceThreshold;
    public float ShakeStrengthX;
    public float ShakeStrengthY;
    public int Vibrato;

    #region Getter And Setters

    public BaseCharacter PlayerOne { get { return playerOne; } }
    public BaseCharacter PlayerTwo { get { return playerTwo; } }

    #endregion

    #region GlobalGameValues

    public static float HealthAnimationDuration;
    public static float EnhanceAttackThresholdDuration;
    public static float ProjectileActiveDuration;
    public static float HitShakeAnimationMultipler;

    #endregion

    void Awake()
    {
        Instance = this;
        HitShakeAnimationMultipler = hitShakeAnim;
        HealthAnimationDuration = healthAnimDuration;
        EnhanceAttackThresholdDuration = meterBurnThreshold;
        ProjectileActiveDuration = projectileDuration;
        Application.targetFrameRate = FrameRate;
    }
}