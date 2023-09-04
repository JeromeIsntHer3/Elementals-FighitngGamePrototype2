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
    

    [Serializable]
    class PlayerMenuVariables
    {
        public TextMeshProUGUI SelectedCharacterNameText;
        public ColorBlock SelectContainerColor;
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

    [SerializeField] PlayerMenuVariables playerOneVariables;
    [SerializeField] PlayerMenuVariables playerTwoVariables;

    readonly Dictionary<int, PlayerMenuVariables> playerVariables = new();
    readonly Dictionary<PlayableCharacter, CharacterContainerUI> characterContainer = new();

    [Header("Multiplayer")]
    [SerializeField] PlayerInputManager playerInputManager;
    [SerializeField] PlayerInputProxy playerInputPrefab;
    [SerializeField] float countdownDuration;
    [SerializeField] TextMeshProUGUI readyCountdownText;

    float countdownTime;
    bool playersAreReady = false;
    bool countReady = false;
    Coroutine cr;

    public Color PlayerOneColor {  get { return playerOneVariables.SelectContainerColor.normalColor; } }
    public Color PlayerTwoColor { get { return playerTwoVariables.SelectContainerColor.normalColor; } }


    void Awake()
    {
        Instance = this;   
    }

    void Start()
    {
        playerVariables.Add(0, playerOneVariables);
        playerVariables.Add(1, playerTwoVariables);

        characterContainer.Add(PlayableCharacter.LeafRanger, rangerContainer);
        characterContainer.Add(PlayableCharacter.FireKnight, knightContainer);
        characterContainer.Add(PlayableCharacter.MetalBladekeeper, bladekeeperContainer);
        characterContainer.Add(PlayableCharacter.CrystalMauler, maulerContainer);

        playerInputManager.playerPrefab = playerInputPrefab.gameObject;
    }

    void OnEnable()
    {
        UIManager.OnGoToCharacterSelect += ToCharacterSelect;
        UIManager.OnGoToMenu += ToMainMenu;
    }

    void OnDisable()
    {
        UIManager.OnGoToCharacterSelect -= ToCharacterSelect;
        UIManager.OnGoToMenu -= ToMainMenu;
    }

    void Update()
    {
        if (!countReady) return;
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            readyCountdownText.text = countdownTime.ToString("0");
        }
        else
        {
            countReady = false;
            playersAreReady = true;
        }
    }

    void ToCharacterSelect(object sender, EventArgs args)
    {
        UIManager.OnSelectCharacter += OnSelectCharacter;
        UIManager.OnConfirmCharacter += OnConfirmCharacter;

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

        UIManager.OnSelectCharacter -= OnSelectCharacter;
        UIManager.OnConfirmCharacter -= OnConfirmCharacter;

        GameManager.Instance.GetPlayerProxy(0).OnDeselect -= OnPlayerPressBack;
        GameManager.Instance.GetPlayerProxy(1).OnDeselect -= OnPlayerPressBack;

        Destroy(GameManager.Instance.GetPlayerInput(0).gameObject);
        Destroy(GameManager.Instance.GetPlayerInput(1).gameObject);

        GameManager.Instance.ClearPlayerInputAndProxies(0);
        GameManager.Instance.ClearPlayerInputAndProxies(1);
    }

    void OnPlayerPressBack(object sender, int index)
    {
        if (GameManager.GameState != GameState.CharacterSelect) return;
        if (playerVariables[index].IsReady)
        {
            countReady = false;
            countdownTime = countdownDuration;
            if(cr != null) StopCoroutine(cr);
            playerVariables[index].SetPlayerReady(false);
            playerVariables[index].PlayerReadyText.gameObject.SetActive(false);
            GameManager.Instance.GetPlayerProxy(index).SetSelectionState(true);
            return;
        }
        UIManager.OnGoToMenu?.Invoke(this, EventArgs.Empty);
    }

    void OnSelectCharacter(object sender, UIManager.OnSelectCharacterArgs args)
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
        playerMenu.SelectedCharacterNameText.text = container.pb_CharacterName;

        Transform spawnedT = Instantiate(container.pb_Info.DisplayPrefab, playerMenu.DisplaySpawn, false).transform;
        spawnedT.localPosition = container.pb_Info.RelativeSpawn;
    }

    void OnConfirmCharacter(object sender, UIManager.OnSelectCharacterArgs args)
    {
        if (GameManager.GameState != GameState.CharacterSelect) return;
        PlayerMenuVariables playerMenu = playerVariables[args.PlayerIndex];

        GameManager.Instance.GetPlayerProxy(args.PlayerIndex).SetSelectionState(false);
        playerMenu.PlayerReadyText.gameObject.SetActive(true);
        playerMenu.SetPlayerReady(true);

        foreach(var player in playerVariables.Values)
        {
            if (!player.IsReady) return;
        }

        playersAreReady = false;
        countReady = true;
        countdownTime = countdownDuration;
        cr = StartCoroutine(DelayReadyUp());
        readyCountdownText.gameObject.SetActive(true);
    }

    IEnumerator DelayReadyUp()
    {
        yield return new WaitUntil(() => playersAreReady);

        readyCountdownText.gameObject.SetActive(false);

        GameManager.OnPlayersReady?.Invoke(this, new GameManager.OnPlayersReadyArgs
        {
            PlayerOneInput = GameManager.Instance.GetPlayerInput(0),
            PlayerOnePrefab = playerVariables[0].CurrentContainer.pb_Info.GamePrefab,
            PlayerOneSpawnPos = playerVariables[0].CurrentContainer.pb_Info.RelativeSpawn,

            PlayerTwoInput = GameManager.Instance.GetPlayerInput(1),
            PlayerTwoPrefab = playerVariables[1].CurrentContainer.pb_Info.GamePrefab,
            PlayerTwoSpawnPos = playerVariables[1].CurrentContainer.pb_Info.RelativeSpawn
        });
    }

    public void ClearDisplays()
    {
        if (playerOneVariables.DisplaySpawn.childCount > 0) Utils.DestroyChildren(playerOneVariables.DisplaySpawn);
        if (playerTwoVariables.DisplaySpawn.childCount > 0) Utils.DestroyChildren(playerTwoVariables.DisplaySpawn);

        playerOneVariables.PlayerReadyText.gameObject.SetActive(false);
        playerTwoVariables.PlayerReadyText.gameObject.SetActive(false);
    }
}

public enum PlayableCharacter
{
    LeafRanger, FireKnight, MetalBladekeeper, CrystalMauler
}