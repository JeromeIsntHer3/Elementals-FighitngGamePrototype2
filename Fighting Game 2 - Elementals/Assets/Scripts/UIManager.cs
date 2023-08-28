using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] MeterUI meterOne;
    [SerializeField] MeterUI meterTwo;

    [SerializeField] HealthBarUI healthBarOne;
    [SerializeField] HealthBarUI healthBarTwo;

    [SerializeField] BaseCharacter playerOne;
    [SerializeField] BaseCharacter playerTwo;


    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        if(playerOne != null)
        {
            playerOne.GetComponent<BaseCharacterAttacks>().OnMeterValueChanged += meterOne.OnMeterUsed;
            playerOne.GetComponent<BaseCharacterHealth>().OnHealthChanged += healthBarOne.OnHealthDepleted;
        }

        if(playerTwo != null)
        {
            playerTwo.GetComponent<BaseCharacterAttacks>().OnMeterValueChanged += meterTwo.OnMeterUsed;
            playerTwo.GetComponent<BaseCharacterHealth>().OnHealthChanged += healthBarTwo.OnHealthDepleted;
        }
    }
}