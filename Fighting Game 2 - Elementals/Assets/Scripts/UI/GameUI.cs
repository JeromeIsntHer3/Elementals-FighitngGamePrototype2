using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : BaseMenuUI
{
    public static GameUI Instance;

    [Header("Health Bar")]
    [SerializeField] HealthBarUI playerOneHealthBar;
    [SerializeField] HealthBarUI playerTwoHealthBar;

    [Header("Meter Bar")]
    [SerializeField] MeterUI playerOneMeter;
    [SerializeField] MeterUI playerTwoMeter;

    [Header("Combo Counter")]
    [SerializeField] ComboUI playerOneCombo;
    [SerializeField] ComboUI playerTwoCombo;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float countdownDuration;

    bool startCountdown;
    float currentTime;
    int round = 1;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTime = countdownDuration;
    }

    void OnEnable()
    {
        MenuSceneManager.OnGoToGame += StartRound;
    }

    void OnDisable()
    {
        MenuSceneManager.OnGoToGame -= StartRound;
    }

    void Update()
    {
        if (!startCountdown) return;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            startCountdown = false;
            currentTime = countdownDuration;
        }

        SetTimerText();
    }

    void SetTimerText()
    {
        countdownText.text = currentTime.ToString("0");
    }

    public void StartRound(object sender, EventArgs args)
    {
        StartCoroutine(Countdown());
    }

    public void SubscribeGameEvents(BaseCharacter playerOne, BaseCharacter playerTwo)
    {
        playerOne.GetComponent<BaseCharacterAttacks>().OnMeterValueChanged += playerOneMeter.OnMeterUsed;
        playerOne.GetComponent<BaseCharacterHealth>().OnHealthChanged += playerOneHealthBar.OnHealthDepleted;
        playerOne.OnHitCombo += playerOneCombo.SetCombo;
        playerOne.OnHitType += playerOneCombo.SetHitType;

        playerTwo.GetComponent<BaseCharacterAttacks>().OnMeterValueChanged += playerTwoMeter.OnMeterUsed;
        playerTwo.GetComponent<BaseCharacterHealth>().OnHealthChanged += playerTwoHealthBar.OnHealthDepleted;
        playerTwo.OnHitCombo += playerTwoCombo.SetCombo;
        playerTwo.OnHitType += playerTwoCombo.SetHitType;
    }

    public void UnsubscribeGameEvents(BaseCharacter playerOne, BaseCharacter playerTwo)
    {
        playerOne.GetComponent<BaseCharacterAttacks>().OnMeterValueChanged -= playerOneMeter.OnMeterUsed;
        playerOne.GetComponent<BaseCharacterHealth>().OnHealthChanged -= playerOneHealthBar.OnHealthDepleted;
        playerOne.OnHitCombo -= playerOneCombo.SetCombo;
        playerOne.OnHitType -= playerOneCombo.SetHitType;

        playerTwo.GetComponent<BaseCharacterAttacks>().OnMeterValueChanged -= playerTwoMeter.OnMeterUsed;
        playerTwo.GetComponent<BaseCharacterHealth>().OnHealthChanged -= playerTwoHealthBar.OnHealthDepleted;
        playerTwo.OnHitCombo -= playerTwoCombo.SetCombo;
        playerTwo.OnHitType -= playerTwoCombo.SetHitType;
    }

    IEnumerator Countdown()
    {
        countdownText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countdownText.text = "ROUND " + round;
        yield return new WaitForSeconds(1);
        countdownText.text = "READY?";
        yield return new WaitForSeconds(1);
        countdownText.text = "FIGHT!";
        GameManager.Instance.EnablePlayerInput(true);
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);
    }
}