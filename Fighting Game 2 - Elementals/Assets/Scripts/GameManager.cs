using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int FrameRate;
    [SerializeField] BaseCharacter playerOne;
    [SerializeField] BaseCharacter playerTwo;
    public float DistanceThreshold;
    public float strength;
    public int vibrato;

    #region Getter And Setters

    public BaseCharacter PlayerOne { get { return playerOne; } }
    public BaseCharacter PlayerTwo { get { return playerTwo; } }

    #endregion

    #region GlobalGameValues

    public static float HealthAnimationDuration;

    #endregion

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = FrameRate;
    }

    public float DistBetweenPlayers()
    {
        return Vector2.Distance(playerOne.transform.position, playerTwo.transform.position);
    }
}