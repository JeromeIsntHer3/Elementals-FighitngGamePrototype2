using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public class OnSelectCharacterArgs : EventArgs
    {
        public PlayableCharacter Character;
        public int PlayerIndex;
    }
    public static EventHandler<OnSelectCharacterArgs> OnSelectCharacter;
    public static EventHandler<OnSelectCharacterArgs> OnConfirmCharacter;

    public static EventHandler OnGoToCharacterSelect;
    public static EventHandler OnGoToMenu;
    public static EventHandler OnGoToGame;
    public static EventHandler<int> OnGamePause;

    [SerializeField] MultiplayerEventSystem menuEventSystem;

    [Header("Menus")]
    [SerializeField] MainMenuUI mainMenu;
    [SerializeField] CharacterSelectMenuUI characterSelect;
    [SerializeField] GameUI gameUI;
    [SerializeField] PauseMenuUI pauseUI;

    [Header("UI Animations")]
    [SerializeField] List<AnimatedUIElement> listOfMenuAnimatedElements;
    [SerializeField] List<AnimatedUIElement> listOfCharacterSelectAnimatedElements;
    [SerializeField] List<AnimatedUIElement> listOfGameAnimatedElements;

    #region Getter & Setters

    public MultiplayerEventSystem MEventSystem { get { return menuEventSystem; } }
    public MainMenuUI MainMenu { get { return mainMenu; } }
    public CharacterSelectMenuUI CharacterSelect { get {  return characterSelect; } }
    public GameUI GamUI { get {  return gameUI; } }
    public PauseMenuUI PauseUI { get { return pauseUI; } }

    #endregion

    Sequence sequence;
    int pauseIndex;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CameraManager.Instance.SetMenuCams();
        mainMenu.Show();
        characterSelect.Show();
    }

    void OnEnable()
    {
        GameManager.OnPlayersReady += OnPlayersReady;
        OnGamePause += GamePaused;
        OnGoToCharacterSelect += ToCharacterSelect;
        OnGoToMenu += ToMainMenu;
        OnGoToGame += ToGame;
    }

    void OnDisable()
    {
        GameManager.OnPlayersReady -= OnPlayersReady;
        OnGamePause -= GamePaused;
        OnGoToCharacterSelect -= ToCharacterSelect;
        OnGoToMenu -= ToMainMenu;
        OnGoToGame -= ToGame;
    }

    void OnPlayersReady(object sender, GameManager.OnPlayersReadyArgs e)
    {
        //foreach (var spawn in displaySpawns)
        //{
        //    if (spawn.childCount > 0) Utils.DestroyChildren(spawn);
        //}
        //indexReadyText[0].gameObject.SetActive(false);
        //indexReadyText[1].gameObject.SetActive(false);
        ////RemoveProxies();

        //foreach(var proxy in playerInputProxy.Values)
        //{
        //    proxy.SetEventSystemState(false);
        //}

        //gameUI.SetActive(true);
        //AnimateUIElementsTransition(listOfCharacterSelectAnimatedElements, listOfGameAnimatedElements, () =>
        //{
        //    characterSelect.SetActive(false);
        //});
    }

    void GamePaused(object sender, int index)
    {
        CanvasGroup group = pauseUI.GetComponent<CanvasGroup>();
        GameManager.Instance.SwitchMapsTo("UI");
        pauseUI.Show();
        group.DOFade(1, .3f);
        GameManager.Instance.GetPlayerProxy(index).SetEventSystem(menuEventSystem);
        GameManager.Instance.GetPlayerProxy(index).SetSelectedObject(pauseUI.ResumeButton.gameObject);
        pauseIndex = index;
        GameManager.Instance.SetGameState(GameState.Pause);
    }

    void ToCharacterSelect(object sender, EventArgs args)
    {
        StartCoroutine(Utils.DelayEndFrame(() =>
        {
            menuEventSystem.playerRoot = pauseUI.gameObject;
            menuEventSystem.gameObject.SetActive(false);
        }));
        
        CameraManager.Instance.SetCharacterSelectCams();

        AnimateUIElementsTransition(listOfMenuAnimatedElements, listOfCharacterSelectAnimatedElements, () =>
        {
            mainMenu.Hide();
            GameManager.Instance.SetSelectionStateOfPlayers(true);
            GameManager.Instance.SetGameState(GameState.CharacterSelect);
        }, sequence);
    }

    void ToMainMenu(object sender, EventArgs args)
    {
        CameraManager.Instance.SetMenuCams();
        mainMenu.Show();

        AnimateUIElementsTransition(listOfCharacterSelectAnimatedElements, listOfMenuAnimatedElements, () =>
        {
            menuEventSystem.gameObject.SetActive(true);
            menuEventSystem.playerRoot = mainMenu.gameObject;
            menuEventSystem.SetSelectedGameObject(mainMenu.PlayButton.gameObject);
            GameManager.Instance.SetGameState(GameState.Menu);
        }, sequence);
    }

    void ToGame(object sender, EventArgs args)
    {
        CameraManager.Instance.SetGamCams();
        gameUI.Show();
        characterSelect.ClearDisplays();

        AnimateUIElementsTransition(listOfCharacterSelectAnimatedElements, listOfGameAnimatedElements, () =>
        {
            menuEventSystem.gameObject.SetActive(true);
            menuEventSystem.SetSelectedGameObject(pauseUI.ResumeButton.gameObject);
            characterSelect.Hide();
            GameManager.Instance.SetGameState(GameState.Game);
        }, sequence);
    }

    public void ClosePauseMenu()
    {
        if (pauseUI.Shown())
        {
            pauseUI.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete(() =>
            {
                pauseUI.Hide();
            });
            GameManager.Instance.SwitchMapsTo("Player");
            GameManager.Instance.GetPlayerProxy(pauseIndex).SetDefaultEventSystem();
            GameManager.Instance.SetGameState(GameState.Game);
            //GameManager.Instance.GetPlayerProxy(pauseIndex).SetSelectedObject(pauseUI.ResumeButton.gameObject);
        }
    }

    public void AnimateUIElementsTransition(List<AnimatedUIElement> from, List<AnimatedUIElement> to, 
        Action onCompleteAction, Sequence sequence, float duration = .75f, float interval = .75f)
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