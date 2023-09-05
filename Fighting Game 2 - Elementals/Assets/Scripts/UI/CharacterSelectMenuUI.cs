using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectMenuUI : BaseMenuUI
{
    public static CharacterSelectMenuUI Instance;

    [SerializeField] CharacterContainerUI rangerContainer;
    [SerializeField] CharacterContainerUI knightContainer;
    [SerializeField] CharacterContainerUI bladekeeperContainer;
    [SerializeField] CharacterContainerUI maulerContainer;
    readonly Dictionary<PlayableCharacter, CharacterContainerUI> characterContainer = new();

    [Serializable]
    class PlayerMenuVariables
    {
        public TextMeshProUGUI SelectedCharacterNameText;
        public ColorBlock SelectContainerColor = new()
        {
            selectedColor = Color.white,
            disabledColor = Color.white,
            highlightedColor = Color.white,
            normalColor = Color.white,
            pressedColor = Color.white,
            colorMultiplier = 3f,
            fadeDuration = .1f
        };
        public Transform DisplaySpawn;
        public TextMeshProUGUI PlayerReadyText;

        CharacterContainerUI containerUI = null;
        public CharacterContainerUI CurrentContainer { get { return containerUI; } }

        bool isReady = false;
        public bool IsReady { get { return isReady; } }

        public void SelectContainerUI(CharacterContainerUI con)
        {
            containerUI = con;
        }

        public void SetPlayerReady(bool state)
        {
            isReady = state;
        }
    }
    [SerializeField] List<PlayerMenuVariables> playerVariables = new();

    [Header("Multiplayer")]
    [SerializeField] PlayerInputManager playerInputManager;
    [SerializeField] PlayerInputProxy playerInputPrefab;
    [SerializeField] float countdownDuration;
    [SerializeField] TextMeshProUGUI readyCountdownText;

    float countdownTime;
    bool playersAreReady = false;
    bool countReady = false;
    Coroutine cr;

    #region Getter & Setters
    public Color PlayerOneColor {  get { return playerVariables[0].SelectContainerColor.normalColor; } }
    public Color PlayerTwoColor { get { return playerVariables[1].SelectContainerColor.normalColor; } }

    public ColorBlock PlayerOneColorBlock { get { return playerVariables[0].SelectContainerColor; } }
    public ColorBlock PlayerTwoColorBlock { get { return playerVariables[1].SelectContainerColor; } }

    #endregion


    void Awake()
    {
        Instance = this;   
    }

    void Start()
    {
        characterContainer.Add(PlayableCharacter.LeafRanger, rangerContainer);
        characterContainer.Add(PlayableCharacter.FireKnight, knightContainer);
        characterContainer.Add(PlayableCharacter.MetalBladekeeper, bladekeeperContainer);
        characterContainer.Add(PlayableCharacter.CrystalMauler, maulerContainer);

        playerInputManager.playerPrefab = playerInputPrefab.gameObject;
    }

    void OnEnable()
    {
        GameManager.OnEnterCharacterSelect += OnEnterChracterSelect;
        GameManager.OnToMenu += ToMainMenu;
    }

    void OnDisable()
    {
        GameManager.OnEnterCharacterSelect -= OnEnterChracterSelect;
        GameManager.OnToMenu -= ToMainMenu;
    }

    void Update()
    {
        if (!countReady) return;
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
        }
        else
        {
            countReady = false;
            playersAreReady = true;
        }
        UpdateCountdownText();
    }

    void UpdateCountdownText()
    {
        readyCountdownText.text = countdownTime.ToString("0");
    }

    void OnEnterChracterSelect(object sender, EventArgs args)
    {
        GameManager.OnSelectCharacter += OnSelectCharacter;
        GameManager.OnConfirmCharacter += OnConfirmCharacter;

        var inputOne = playerInputManager.JoinPlayer(0, default, "Keyboard");
        var inputTwo = playerInputManager.JoinPlayer(1, default, "Controller");

        GameManager.Instance.SetupPlayerInputAndProxies(0, inputOne);
        GameManager.Instance.SetupPlayerInputAndProxies(1, inputTwo);

        GameManager.Instance.GetPlayerProxy(0).OnDeselect += OnPlayerPressBack;
        GameManager.Instance.GetPlayerProxy(1).OnDeselect += OnPlayerPressBack;
    }

    void ToMainMenu(object sender, EventArgs args)
    {
        ClearDisplays();

        GameManager.OnSelectCharacter -= OnSelectCharacter;
        GameManager.OnConfirmCharacter -= OnConfirmCharacter;

        for(int i = 0; i < 2; i++)
        {
            GameManager.Instance.GetPlayerProxy(i).OnDeselect -= OnPlayerPressBack;
            GameManager.Instance.ClearPlayerInputAndProxies(i);
            playerVariables[i].SetPlayerReady(false);
            playerVariables[i].CurrentContainer.Deselect(i);
            playerVariables[i].SelectContainerUI(null);
        }
    }

    void OnPlayerPressBack(object sender, int index)
    {
        if (GameManager.GameState != GameState.CharacterSelect) return;
        if (playerVariables[index].IsReady)
        {
            countReady = false;
            countdownTime = countdownDuration;
            readyCountdownText.gameObject.SetActive(false);
            if(cr != null) StopCoroutine(cr);
            playerVariables[index].SetPlayerReady(false);
            playerVariables[index].PlayerReadyText.gameObject.SetActive(false);
            GameManager.Instance.GetPlayerProxy(index).SetSelectionState(true);
            return;
        }
        GameManager.OnToMenu?.Invoke(this, EventArgs.Empty);
    }

    void OnSelectCharacter(object sender, GameManager.OnSelectCharacterArgs args)
    {
        PlayerMenuVariables playerMenu = playerVariables[args.PlayerIndex];
        CharacterContainerUI container = characterContainer[args.Character];

        if(playerMenu.CurrentContainer != null)
        {
            playerMenu.CurrentContainer.Deselect(args.PlayerIndex);
            if (playerMenu.DisplaySpawn.childCount > 0) Utils.DestroyChildren(playerMenu.DisplaySpawn);
        }

        playerMenu.SelectContainerUI(container);
        playerMenu.CurrentContainer.Select(playerMenu.SelectContainerColor, args.PlayerIndex);
        playerMenu.SelectedCharacterNameText.text = container.Info.Name;

        Transform spawnedT = Instantiate(container.Info.DisplayPrefab, playerMenu.DisplaySpawn, false).transform;
        spawnedT.localPosition = container.Info.RelativeSpawn;
    }

    void OnConfirmCharacter(object sender, GameManager.OnSelectCharacterArgs args)
    {
        if (GameManager.GameState != GameState.CharacterSelect) return;
        PlayerMenuVariables playerVar = playerVariables[args.PlayerIndex];

        GameManager.Instance.GetPlayerProxy(args.PlayerIndex).SetSelectionState(false);
        playerVar.PlayerReadyText.gameObject.SetActive(true);
        playerVar.SetPlayerReady(true);

        foreach(var player in playerVariables)
        {
            if (!player.IsReady) return;
        }

        playersAreReady = false;
        countReady = true;
        countdownTime = countdownDuration;
        UpdateCountdownText();
        readyCountdownText.gameObject.SetActive(true);
        cr = StartCoroutine(DelayReadyUp());
    }

    IEnumerator DelayReadyUp()
    {
        yield return new WaitUntil(() => playersAreReady);

        readyCountdownText.gameObject.SetActive(false);
        GameManager.OnToGame?.Invoke(this, new GameManager.CharacterInfoArgs
        {
            PlayerOneInput = GameManager.Instance.GetPlayerInput(0),
            PlayerOneInfo = playerVariables[0].CurrentContainer.Info,

            PlayerTwoInput = GameManager.Instance.GetPlayerInput(1),
            PlayerTwoInfo = playerVariables[1].CurrentContainer.Info
        });

        ClearDisplays();
    }

    public void ClearDisplays()
    {
        for(int i = 0; i < 2; i++)
        {
            playersAreReady = false;
            playerVariables[i].SetPlayerReady(false);
            playerVariables[i].SelectedCharacterNameText.text = string.Empty;
            playerVariables[i].CurrentContainer.Deselect(i);
            if (playerVariables[i].DisplaySpawn.childCount > 0) Utils.DestroyChildren(playerVariables[i].DisplaySpawn);
            playerVariables[i].PlayerReadyText.gameObject.SetActive(false);
        }
    }
}

public enum PlayableCharacter
{
    LeafRanger, FireKnight, MetalBladekeeper, CrystalMauler
}