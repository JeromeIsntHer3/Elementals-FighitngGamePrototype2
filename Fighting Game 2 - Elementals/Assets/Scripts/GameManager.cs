using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int FrameRate;

    #region GlobalGameValues

    public static float HealthAnimationDuration;

    #endregion

    void Awake()
    {
        Application.targetFrameRate = FrameRate;
    }
}