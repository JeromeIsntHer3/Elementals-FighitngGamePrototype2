using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] MultiplayerEventSystem menuEventSystem;

    [Header("Menus")]
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] CharacterSelectMenuUI characterSelectUI;
    [SerializeField] GameUI gameUI;
    [SerializeField] PauseMenuUI pauseUI;
    [SerializeField] SettingsUI settingsUI;
    [SerializeField] GameOverMenuUI gameOverUI;

    #region Getter & Setters

    public MultiplayerEventSystem MEventSystem { get { return menuEventSystem; } }
    public MainMenuUI MainMenu { get { return mainMenuUI; } }
    public CharacterSelectMenuUI CharacterSelect { get {  return characterSelectUI; } }
    public GameUI GamUI { get {  return gameUI; } }
    public PauseMenuUI PauseUI { get { return pauseUI; } }

    #endregion

    Sequence sequence;
    int pauseIndex;

    void Awake()
    {
        Instance = this;

        mainMenuUI.gameObject.SetActive(true);
        characterSelectUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(true);
        settingsUI.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(true);
    }

    void Start()
    {
        CameraManager.Instance.SetMenuCams();
        characterSelectUI.Hide();
        gameUI.Hide();
        pauseUI.Hide();
        settingsUI.Hide();
        gameOverUI.Hide();
        mainMenuUI.Show();
    }

    void OnEnable()
    {
        GameManager.OnToMenu += OnGoToMainMenu;
        GameManager.OnToCharacterSelect += OnGoToCharSelect;
        GameManager.OnGamePause += GamePaused;
        GameManager.OnToGame += OnGoToGame;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRematch += OnGoToGame;
    }

    void OnDisable()
    {
        GameManager.OnToMenu -= OnGoToMainMenu;
        GameManager.OnToCharacterSelect -= OnGoToCharSelect;
        GameManager.OnGamePause -= GamePaused;
        GameManager.OnToGame -= OnGoToGame;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRematch += OnGoToGame;
    }

    void OnGoToMainMenu(object sender, EventArgs args)
    {
        CameraManager.Instance.SetMenuCams();
        mainMenuUI.Show();

        AudioManager.Instance.SetBGM(LevelBGM.MainMenu);

        switch (GameManager.GameState)
        {
            case GameState.CharacterSelect:

                AnimateUIElementsTransition(characterSelectUI.AnimatedElements, mainMenuUI.AnimatedElements, sequence, () =>
                {
                    menuEventSystem.gameObject.SetActive(true);
                    menuEventSystem.playerRoot = mainMenuUI.gameObject;
                    menuEventSystem.SetSelectedGameObject(null);
                    StartCoroutine(Utils.DelayEndFrame(() =>
                    {
                        menuEventSystem.SetSelectedGameObject(mainMenuUI.PlayButton.gameObject);
                    }));

                    characterSelectUI.Hide();
                    GameManager.Instance.SetGameState(GameState.Menu);
                });

                break;

            case GameState.Pause:

                GameUI.Instance.StopGame();
                ClosePauseMenu();
                GameManager.Instance.RemovePlayerGameObjects();
                GameManager.Instance.ClearPlayerInputAndProxies(0);
                GameManager.Instance.ClearPlayerInputAndProxies(1);
                AnimateUIElementsTransition(gameUI.AnimatedElements, mainMenuUI.AnimatedElements, sequence, () =>
                {
                    menuEventSystem.gameObject.SetActive(true);
                    menuEventSystem.playerRoot = mainMenuUI.gameObject;
                    menuEventSystem.SetSelectedGameObject(null);
                    StartCoroutine(Utils.DelayEndFrame(() =>
                    {
                        menuEventSystem.SetSelectedGameObject(mainMenuUI.PlayButton.gameObject);
                        gameUI.Hide();
                        GameManager.Instance.SetGameState(GameState.Menu);
                    }));
                    
                });

                break;

            case GameState.GameOver:

                GameManager.Instance.RemovePlayerGameObjects();
                GameManager.Instance.ClearPlayerInputAndProxies(0);
                GameManager.Instance.ClearPlayerInputAndProxies(1);
                AnimateUIElementsTransition(gameOverUI.AnimatedElements, mainMenuUI.AnimatedElements, sequence, () =>
                {
                    menuEventSystem.gameObject.SetActive(true);
                    menuEventSystem.playerRoot = mainMenuUI.gameObject;
                    menuEventSystem.SetSelectedGameObject(null);
                    StartCoroutine(Utils.DelayEndFrame(() =>
                    {
                        menuEventSystem.SetSelectedGameObject(mainMenuUI.PlayButton.gameObject);
                        gameUI.Hide();
                        GameManager.Instance.SetGameState(GameState.Menu);
                    }));
                });

                break;
        }
    }

    void OnGoToCharSelect(object sender, EventArgs args)
    {
        CameraManager.Instance.SetCharacterSelectCams();
        characterSelectUI.Show();

        AudioManager.Instance.SetBGM(LevelBGM.CharacterSelect);

        switch (GameManager.GameState)
        {
            case GameState.Menu:

                AnimateUIElementsTransition(mainMenuUI.AnimatedElements, characterSelectUI.AnimatedElements, sequence, () =>
                {
                    StartCoroutine(Utils.DelayEndFrame(() =>
                    {
                        menuEventSystem.playerRoot = pauseUI.gameObject;
                        menuEventSystem.gameObject.SetActive(false);
                    }));

                    mainMenuUI.Hide();
                    GameManager.OnEnterCharacterSelect?.Invoke(this, EventArgs.Empty);
                    GameManager.Instance.SetSelectionStateOfPlayers(true);
                    GameManager.Instance.SetGameState(GameState.CharacterSelect);
                });

                break;

            case GameState.Pause:

                GameUI.Instance.StopGame();
                ClosePauseMenu();
                GameManager.Instance.RemovePlayerGameObjects();
                GameManager.Instance.ClearPlayerInputAndProxies(0);
                GameManager.Instance.ClearPlayerInputAndProxies(1);

                AnimateUIElementsTransition(gameUI.AnimatedElements, characterSelectUI.AnimatedElements, sequence, () =>
                {
                    GameManager.OnEnterCharacterSelect.Invoke(this, EventArgs.Empty);
                    gameUI.Hide();
                    GameManager.Instance.SetGameState(GameState.CharacterSelect);
                });

                break;

            case GameState.GameOver:

                GameManager.Instance.RemovePlayerGameObjects();
                GameManager.Instance.ClearPlayerInputAndProxies(0);
                GameManager.Instance.ClearPlayerInputAndProxies(1);

                AnimateUIElementsTransition(gameOverUI.AnimatedElements, characterSelectUI.AnimatedElements, sequence, () =>
                {
                    GameManager.OnEnterCharacterSelect.Invoke(this, EventArgs.Empty);
                    gameOverUI.Hide();
                    GameManager.Instance.SetGameState(GameState.CharacterSelect);
                });

                break;
        }
    }

    void OnGoToGame(object sender, EventArgs args)
    {
        CameraManager.Instance.SetGameCams();
        gameUI.Show();

        AudioManager.Instance.SetBGM(LevelBGM.Battle);

        switch (GameManager.GameState)
        {
            case GameState.CharacterSelect:

                //GameManager.OnToGame?.Invoke(this, EventArgs.Empty);
                AnimateUIElementsTransition(characterSelectUI.AnimatedElements, gameUI.AnimatedElements, sequence, () =>
                {
                    characterSelectUI.Hide();
                    GameManager.OnEnterGame?.Invoke(this, EventArgs.Empty);
                    GameManager.Instance.SetGameState(GameState.Game);
                });

                break;
            case GameState.GameOver:
                gameUI.ResetStage();
                AnimateUIElementsTransition(gameOverUI.AnimatedElements, gameUI.AnimatedElements, sequence, () =>
                {
                    gameOverUI.Hide();
                });
                break;
        }
    }

    void OnGameOver(object sender, EventArgs args)
    {
        gameOverUI.Show();
        AudioManager.Instance.SetBGM(LevelBGM.GameOver);
        AnimateUIElementsTransition(gameUI.AnimatedElements, gameOverUI.AnimatedElements, sequence, () =>
        {
            gameUI.Hide();
            GameManager.Instance.EnablePlayerInput(true);
            GameManager.Instance.SwitchMapsTo(GameManager.UIInput);
            //Setup Proxies to select their canvases
            
            for(int i = 0; i < 2; i++)
            {
                GameOverUISide playerOneSide = gameOverUI.GetUISide(i);
                GameManager.Instance.GetPlayerProxy(i).SetRootObject(playerOneSide.RootObject);
                GameManager.Instance.GetPlayerProxy(i).SetSelectedObject(playerOneSide.SelectObject);
            }
        });
    }

    void GamePaused(object sender, int index)
    {
        OpenPauseMenu(index);
    }

    public void OpenPauseMenu(int index)
    {
        CanvasGroup group = pauseUI.GetComponent<CanvasGroup>();
        GameManager.Instance.SwitchMapsTo(GameManager.UIInput);
        pauseUI.Show();
        group.DOFade(1, .3f);
        menuEventSystem.gameObject.SetActive(true);
        GameManager.Instance.GetPlayerProxy(index).SetEventSystem(menuEventSystem);
        GameManager.Instance.GetPlayerProxy(index).SetSelectedObject(pauseUI.ResumeButton.gameObject);
        pauseIndex = index;
        GameManager.Instance.SetGameState(GameState.Pause);
    }

    public void ClosePauseMenu()
    {
        if (pauseUI.Shown())
        {
            pauseUI.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete(() =>
            {
                pauseUI.Hide();
            }); 
            GameManager.Instance.SwitchMapsTo(GameManager.PlayerInput);
            GameManager.Instance.GetPlayerProxy(pauseIndex).SetDefaultEventSystem();
            GameManager.Instance.SetGameState(GameState.Game);
            menuEventSystem.gameObject.SetActive(false);
        }
    }

    public void AnimateUIElementsTransition(List<AnimatedUIElement> from, List<AnimatedUIElement> to, Sequence sequence,
        Action onCompleteAction, float duration = .75f, float interval = .75f)
    {
        sequence?.Kill();
        sequence = DOTween.Sequence();

        foreach (var element in from)
        {
            sequence.Join(element.Rect.DOAnchorPos(element.AnimateToPos, duration));
        }
        sequence.AppendInterval(interval);
        foreach (var element in to)
        {
            sequence.Join(element.Rect.DOAnchorPos(element.OriginalPosition, duration));
        }
        sequence.OnComplete(() =>
        {
            onCompleteAction?.Invoke();
        });
    }
}