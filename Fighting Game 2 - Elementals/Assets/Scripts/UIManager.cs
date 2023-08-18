using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] HealthBarUI playerOneHealthBar;
    [SerializeField] HealthBarUI playerTwoHealthBar;

    [SerializeField] MeterUI playerOneMeterBar;
    [SerializeField] MeterUI playerTwoMeterBar;

    void Awake()
    {
        Invoke(nameof(DelayAwake), .05f);
    }

    void DelayAwake()
    {
        playerOneHealthBar.Setup(GameManager.Instance.PlayerOne.GetComponent<BaseCharacterHealth>());
        playerTwoHealthBar.Setup(GameManager.Instance.PlayerTwo.GetComponent<BaseCharacterHealth>());

        playerOneMeterBar.Setup(GameManager.Instance.PlayerOne.GetComponent<BaseCharacterAttacks>());
        playerTwoMeterBar.Setup(GameManager.Instance.PlayerTwo.GetComponent<BaseCharacterAttacks>());
    }
}