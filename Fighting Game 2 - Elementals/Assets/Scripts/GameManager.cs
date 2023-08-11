using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int FrameRate;

    void Awake()
    {
        Application.targetFrameRate = FrameRate;
    }
}