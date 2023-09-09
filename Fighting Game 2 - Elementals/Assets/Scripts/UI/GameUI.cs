using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseMenuUI
{
    public static GameUI Instance;

    [Header("Icons")]
    [SerializeField] Image playerOneIcon;
    [SerializeField] Image playerTwoIcon;

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

    Coroutine GameOverCR;
    Coroutine StartRoundCR;
    Coroutine RoundOverCR;

    readonly List<int> playerWins = new();

    bool isGameRunning;
    bool isTimeOut;
    float currentTime;
    int round = 0;
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
        GameManager.OnEnterGame += OnEnterGame;
        OnPlayerDeath += PlayerDeath;
    }

    void OnDisable()
    {
        GameManager.OnEnterGame -= OnEnterGame;
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

    void OnEnterGame(object sender, EventArgs args)
    {
        currentTime = countdownDuration;
        UpdateTimerText();
        StartRoundCR = StartCoroutine(RoundStartCountdown());
    }

    void PlayerDeath(object sender, int index)
    {
        playerLostIndex = index;
        RoundOver();
    }

    public void SetupIcons(Sprite one, Sprite two)
    {
        playerOneIcon.sprite = one;
        playerTwoIcon.sprite = two;
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

    public void ResetStage()
    {
        currentTime = countdownDuration;
        UpdateTimerText();
        GameManager.Instance.ResetPlayers();
        StartRoundCR = StartCoroutine(RoundStartCountdown());
    }

    IEnumerator RoundStartCountdown()
    {
        GameManager.Instance.SetGameState(GameState.Game);
        yield return new WaitForSeconds(1);
        centreText.gameObject.SetActive(true);
        centreText.text = "ROUND " + (round + 1);
        yield return new WaitForSeconds(1);
        centreText.text = "READY?";
        yield return new WaitForSeconds(1);
        centreText.text = "FIGHT!";
        yield return new WaitForSeconds(1f);
        centreText.gameObject.SetActive(false);
        GameManager.Instance.EnablePlayerInput(true);
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
                    scoreList[round].SetColor(color);
                    playerWins[0]++;
                    break;
                case 1:
                    color = CharacterSelectMenuUI.Instance.PlayerTwoColor;
                    scoreList[round].SetColor(color);
                    playerWins[1]++;
                    break;
                case -1:
                    scoreList[round].SetColor(Color.magenta);
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
                    scoreList[round].SetColor(color);
                    playerWins[0]++;
                    break;
                case 0:
                    color = CharacterSelectMenuUI.Instance.PlayerTwoColor;
                    scoreList[round].SetColor(color);
                    playerWins[1]++;
                    break;
                case -1:
                    scoreList[round].SetColor(Color.magenta);
                    playerWins[0]++;
                    playerWins[1]++;
                    break;
            }
        }

        RoundOverCR = StartCoroutine(RoundOverSequence());
    }

    IEnumerator RoundOverSequence()
    {
        GameManager.Instance.SetGameState(GameState.GameOver);
        scoreList[round].Show(true);
        round++;
        centreText.gameObject.SetActive(true);
        centreText.text = "K.O.";
        isTimeOut = false;
        yield return new WaitForSeconds(1);
        centreText.text = "";
        yield return new WaitForSeconds(1f);

        if (round >= 2)
        {
            bool bothPlayersWin = playerWins[0] >= 2 && playerWins[1] >= 2;
            if (bothPlayersWin)
            {
                GameOverCR = StartCoroutine(GameOverSequence("Draw!"));
            }
            else
            {
                if (playerWins[0] >= 2)
                {
                    GameOverCR = StartCoroutine(GameOverSequence("Player 1 Wins!"));
                }
                else if (playerWins[1] >= 2)
                {
                    GameOverCR = StartCoroutine(GameOverSequence("Player 2 Wins!"));
                }
            }
            if (aPlayerWon) yield break;
        }

        ResetStage();
    }

    IEnumerator GameOverSequence(string gameOverText)
    {
        aPlayerWon = true;
        yield return new WaitForSeconds(1);
        centreText.text = gameOverText;
        yield return new WaitForSeconds(1);
        GameManager.OnGameOver?.Invoke(this, EventArgs.Empty);
        ResetValues();
    }

    void ResetValues()
    {
        round = 0;
        playerWins[0] = 0;
        playerWins[1] = 0;
        foreach(var score in scoreList)
        {
            score.SetColor(Color.white);
            score.Show(false);
        }
        centreText.text = "";
    }

    public void StopGame()
    {
        if(StartRoundCR != null) StopCoroutine(StartRoundCR);
        if(RoundOverCR != null) StopCoroutine(RoundOverCR);
        if(GameOverCR != null) StopCoroutine(GameOverCR);

        isGameRunning = false;
    }
}