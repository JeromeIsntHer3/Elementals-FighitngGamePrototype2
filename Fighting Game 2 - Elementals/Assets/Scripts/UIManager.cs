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
        //Invoke(nameof(LateAwake), .1f);
    }

    void OnEnable()
    {
        playerOne.GetComponent<BaseCharacterAttacks>().OnMeterUsed += meterOne.OnMeterUsed;
        playerTwo.GetComponent<BaseCharacterAttacks>().OnMeterUsed += meterTwo.OnMeterUsed;

        playerOne.GetComponent<BaseCharacterHealth>().OnHealthChanged += healthBarOne.OnHealthDepleted;
        playerTwo.GetComponent<BaseCharacterHealth>().OnHealthChanged += healthBarTwo.OnHealthDepleted;
    }
}