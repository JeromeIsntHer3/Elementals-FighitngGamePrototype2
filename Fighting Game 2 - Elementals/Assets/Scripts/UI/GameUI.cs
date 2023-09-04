using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    [SerializeField] TextMeshProUGUI centreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float countdownDuration;

    [Header("Scores")]
    [SerializeField] List<ScoreSoloUI> scoreList;

    readonly List<int> playerWins = new();

    bool isGameRunning;
    bool isTimeOut;
    float currentTime;
    int round = 1;
    int playerLostIndex = -1;
    bool aPlayerWon;

    public static EventHandler<int> OnPlayerDeath;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerWins.Add(0);
        playerWins.Add(0);
    }

    void OnEnable()
    {
        UIManager.OnGoToGame += OnGoToGame;
        OnPlayerDeath += PlayerDeath;
    }

    void OnDisable()
    {
        UIManager.OnGoToGame -= OnGoToGame;
        OnPlayerDeath -= PlayerDeath;
    }

    void Update()
    {
        if (GameManager.GameState == GameState.Pause) return;
        if (!isGameRunning) return;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            isGameRunning = false;
            isTimeOut = true;
            currentTime = countdownDuration;
            RoundOver();
        }
        UpdateTimerText();
    }

    void OnGoToGame(object sender, EventArgs args)
    {
        currentTime = countdownDuration;
        UpdateTimerText();
        StartCoroutine(RoundStartCountdown());
    }

    void PlayerDeath(object sender, int index)
    {
        playerLostIndex = index;
        RoundOver();
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

    void UpdateTimerText()
    {
        timerText.text = currentTime.ToString("0");
    }

    void ResetUI()
    {
        currentTime = countdownDuration;
        UpdateTimerText();
        GameManager.Instance.ResetPlayers();
        StartCoroutine(RoundStartCountdown());
    }

    IEnumerator RoundStartCountdown()
    {
        yield return new WaitForSeconds(1);
        centreText.gameObject.SetActive(true);
        centreText.text = "ROUND " + round;
        yield return new WaitForSeconds(1);
        centreText.text = "READY?";
        yield return new WaitForSeconds(1);
        centreText.text = "FIGHT!";
        GameManager.Instance.EnablePlayerInput(true);
        yield return new WaitForSeconds(.5f);
        centreText.gameObject.SetActive(false);
        isGameRunning = true;
    }

    void RoundOver()
    {
        GameManager.Instance.EnablePlayerInput(false);
        isGameRunning = false;

        Color color;

        if (isTimeOut)
        {
            switch (GameManager.Instance.GetPlayerIndexWinner())
            {
                case 0:
                    color = CharacterSelectMenuUI.Instance.PlayerOneColor;
                    scoreList[round - 1].SetColor(color);
                    playerWins[0]++;
                    break;
                case 1:
                    color = CharacterSelectMenuUI.Instance.PlayerTwoColor;
                    scoreList[round - 1].SetColor(color);
                    playerWins[1]++;
                    break;
                case -1:
                    scoreList[round - 1].SetColor(Color.magenta);
                    playerWins[0]++;
                    playerWins[1]++;
                    break;
            }
        }
        else
        {
            switch (playerLostIndex)
            {
                case 1:
                    color = CharacterSelectMenuUI.Instance.PlayerOneColor;
                    scoreList[round - 1].SetColor(color);
                    playerWins[0]++;
                    break;
                case 0:
                    color = CharacterSelectMenuUI.Instance.PlayerTwoColor;
                    scoreList[round - 1].SetColor(color);
                    playerWins[1]++;
                    break;
                case -1:
                    scoreList[round - 1].SetColor(Color.magenta);
                    playerWins[0]++;
                    playerWins[1]++;
                    break;
            }
        }

        StartCoroutine(RoundOverSequence());
    }

    IEnumerator RoundOverSequence()
    {
        scoreList[round - 1].Show(true);
        round++;
        centreText.gameObject.SetActive(true);
        centreText.text = "K.O.";
        isTimeOut = false;
        yield return new WaitForSeconds(1);
        centreText.text = "";
        yield return new WaitForSeconds(1f);

        if (round - 1 >= 2)
        {
            bool bothPlayersWin = playerWins[0] >= 2 && playerWins[1] >= 2;
            if (bothPlayersWin)
            {
                StartCoroutine(GameOverSequence("Draw!"));
            }
            else
            {
                if (playerWins[0] >= 2)
                {
                    StartCoroutine(GameOverSequence("Player 1 Wins!"));
                }
                else
                {
                    StartCoroutine(GameOverSequence("Player 2 Wins!"));
                }
            }
            if (aPlayerWon) yield break;
        }

        ResetUI();
    }

    IEnumerator GameOverSequence(string gameOverText)
    {
        aPlayerWon = true;
        yield return new WaitForSeconds(1);
        centreText.text = gameOverText;
        yield return new WaitForSeconds(1);
        Debug.Log("Open GameOver Menu");
    }
}