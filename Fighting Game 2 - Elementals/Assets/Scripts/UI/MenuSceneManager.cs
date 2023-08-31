using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    public static MenuSceneManager Instance;

    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject characterSelect;

    [Header("Main Menu")]
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    [Header("Character Select")]
    [SerializeField] List<TextMeshProUGUI> playerCharacterNames;
    [SerializeField] List<ColorBlock> colorBlocks;
    [SerializeField] List<Transform> displaySpawns;
    [SerializeField] List<TextMeshProUGUI> playerReadyTexts;

    [Header("Multiplayer")]
    [SerializeField] PlayerInputManager pim;
    [SerializeField] EventSystem es;
    [SerializeField] GameObject playerPrefab;

    [Header("Character Containers")]
    [SerializeField] CharacterContainerUI ranger;
    [SerializeField] CharacterContainerUI knight;
    [SerializeField] CharacterContainerUI bladekeeper;
    [SerializeField] CharacterContainerUI mauler;

    [Header("UI Animations")]
    [SerializeField] List<AnimatedUIElement> listOfMenuAnimatedElements;
    [SerializeField] List<AnimatedUIElement> listOfCharacterSelectAnimatedElements;

    #region Getter & Setters

    public CharacterContainerUI Ranger { get { return ranger; } }
    public CharacterContainerUI Knight { get { return knight; } }
    public CharacterContainerUI Bladekeeper { get { return bladekeeper; } }
    public CharacterContainerUI Mauler { get { return mauler; } }

    #endregion

    readonly Dictionary<int, CharacterContainerUI> selectedCharacter = new();
    readonly Dictionary<int, ColorBlock> indexColorBlock = new();
    readonly Dictionary<int, TextMeshProUGUI> indexName = new();
    readonly Dictionary<int, Transform> displaySpawn = new();
    readonly Dictionary<int, TextMeshProUGUI> indexReadyText = new();
    readonly Dictionary<int, PlayerInputProxy> playerInputProxy = new();
    readonly Dictionary<int, CharacterContainerUI> confirmedCharacter = new();
    readonly Dictionary<int, bool> playerIsReady = new();
    readonly Dictionary<int, PlayerInput> playerInputs = new();

    PlayerInputProxy proxyOne;
    PlayerInputProxy proxyTwo;

    Sequence sequence;

    void Start()
    {
        Instance = this;

        startButton.onClick.AddListener(ToCharacterSelect);
        settingsButton.onClick.AddListener(ToSettings);
        quitButton.onClick.AddListener(() => { Application.Quit(); });

        pim.playerPrefab = playerPrefab;

        for(int i = 0; i < 2; i++)
        {
            indexColorBlock.Add(i, colorBlocks[i]);
            indexName.Add(i, playerCharacterNames[i]);
            displaySpawn.Add(i, displaySpawns[i]);
            indexReadyText.Add(i, playerReadyTexts[i]);
            playerIsReady.Add(i, false);
            confirmedCharacter.Add(i, null);
        }

        CameraManager.Instance.SetMenuCams();
    }

    void OnEnable()
    {
        GameManager.Instance.OnPlayersReady += OnPlayersReady;
    }

    void OnDisable()
    {
        GameManager.Instance.OnPlayersReady -= OnPlayersReady;
    }

    void OnPlayersReady(object sender, GameManager.OnPlayersReadyArgs e)
    {
        foreach (var spawn in displaySpawns)
        {
            if (spawn.childCount > 0) Utils.DestroyChildren(spawn);
        }

        indexReadyText[0].gameObject.SetActive(false);
        indexReadyText[1].gameObject.SetActive(false);

        RemoveProxies();
    }

    void ToCharacterSelect()
    {
        GameManager.Instance.GameState = GameState.CharacterSelect;

        es.gameObject.SetActive(false);

        //mainMenu.SetActive(false);
        characterSelect.SetActive(true);

        PlayerInput one = pim.JoinPlayer(0, default, "Keyboard");
        PlayerInput two = pim.JoinPlayer(1, default, "Controller");

        playerInputs.Add(0, one);
        playerInputs.Add(1, two);

        AddProxies(one,two);

        CameraManager.Instance.SetCharacterSelectCams();

        characterSelect.SetActive(true);
        sequence?.Kill();
        sequence = DOTween.Sequence();
        foreach (var element in listOfMenuAnimatedElements)
        {
            sequence.Join(element.GetComponent<RectTransform>().DOAnchorPos(element.AnimateToPos, 1));
        }
        sequence.AppendInterval(1f);
        foreach (var element in listOfCharacterSelectAnimatedElements)
        {
            sequence.Join(element.GetComponent<RectTransform>().DOAnchorPos(element.OriginalPosition, 1));
        }
        sequence.OnComplete(() =>
        {
            mainMenu.SetActive(false);
        });
    }

    void ToSettings()
    {

    }

    void BackToMainMenu()
    {
        GameManager.Instance.GameState = GameState.Menu;

        foreach (var spawn in displaySpawns)
        {
            if(spawn.childCount > 0) Utils.DestroyChildren(spawn);
        }

        indexReadyText[0].gameObject.SetActive(false);
        indexReadyText[1].gameObject.SetActive(false);

        playerInputs.Clear();

        RemoveProxies();

        es.gameObject.SetActive(true);

        CameraManager.Instance.SetMenuCams();

        mainMenu.SetActive(true);
        sequence?.Kill();
        sequence = DOTween.Sequence();
        foreach (var element in listOfCharacterSelectAnimatedElements)
        {
            sequence.Join(element.GetComponent<RectTransform>().DOAnchorPos(element.AnimateToPos, 1));
        }
        sequence.AppendInterval(1f);
        foreach (var element in listOfMenuAnimatedElements)
        {
            sequence.Join(element.GetComponent<RectTransform>().DOAnchorPos(element.OriginalPosition, 1));
        }
        sequence.OnComplete(() =>
        {
            characterSelect.SetActive(false);
        });
    }

    void ProxyOneUnconfirmCharacter(object sender, EventArgs args)
    {
        if (GameManager.Instance.GameState != GameState.CharacterSelect) return;
        if (confirmedCharacter[0])
        {
            indexReadyText[0].gameObject.SetActive(false);
            proxyOne.SetSelectionState(true);
            confirmedCharacter[0] = null;
        }
        else
        {
            BackToMainMenu();
        }
    }

    void ProxyTwoUnconfirmCharacter(object sender, EventArgs args)
    {
        if (GameManager.Instance.GameState != GameState.CharacterSelect) return;
        if (confirmedCharacter[1])
        {
            indexReadyText[1].gameObject.SetActive(false);
            proxyTwo.SetSelectionState(true);
            confirmedCharacter[1] = null;
        }
        else
        {
            BackToMainMenu();
        }
    }

    void SelectNewCharacter(CharacterContainerUI c, int playerIndex)
    {
        //Set Values
        selectedCharacter[playerIndex] = c;
        selectedCharacter[playerIndex].Select(indexColorBlock[playerIndex], playerIndex);
        indexName[playerIndex].text = c.pb_CharacterName;
        //Spawn the character Display
        Transform spawnedT = Instantiate(c.pb_Info.DisplayPrefab, displaySpawn[playerIndex], false).transform;
        spawnedT.localPosition = c.pb_Info.RelativeSpawn;
    }

    public void SelectCharacter(CharacterContainerUI con, int playerIndex)
    {
        if(selectedCharacter.ContainsKey(playerIndex))
        {
            if (selectedCharacter[playerIndex])
            {
                selectedCharacter[playerIndex].Deselect(playerIndex);
                if (displaySpawn[playerIndex].childCount > 0) Utils.DestroyChildren(displaySpawn[playerIndex]);
            }
            SelectNewCharacter(con, playerIndex);
        }
        else
        {
            selectedCharacter.Add(playerIndex, con);
            SelectNewCharacter(con, playerIndex);
        }
    }

    public void ConfirmCharacter(CharacterContainerUI c, int playerIndex)
    {
        indexReadyText[playerIndex].gameObject.SetActive(true);
        playerInputProxy[playerIndex].SetSelectionState(false);
        confirmedCharacter[playerIndex] = c;
        playerIsReady[playerIndex] = true;

        foreach (var ready in playerIsReady.Values)
        {
            if (!ready) return;
        }
        StartCoroutine(Utils.DelayEndFrame(PlayersAreReady));
    }

    void PlayersAreReady()
    {
        GameManager.Instance.OnPlayersReady?.Invoke(this, e:
            new(confirmedCharacter[0].pb_Info.RelativeSpawn,
            confirmedCharacter[1].pb_Info.RelativeSpawn,
            confirmedCharacter[0].pb_Info.GamePrefab, confirmedCharacter[1].pb_Info.GamePrefab));
    }

    void AddProxies(PlayerInput one, PlayerInput two)
    {
        proxyOne = one.GetComponent<PlayerInputProxy>().SetupProxy(this, one.playerIndex);
        proxyTwo = two.GetComponent<PlayerInputProxy>().SetupProxy(this, two.playerIndex);

        playerInputProxy.Add(0, proxyOne);
        playerInputProxy.Add(1, proxyTwo);

        proxyOne.OnDeselect += ProxyOneUnconfirmCharacter;
        proxyTwo.OnDeselect += ProxyTwoUnconfirmCharacter;
    }

    void RemoveProxies()
    {
        proxyOne.OnDeselect -= ProxyOneUnconfirmCharacter;
        proxyTwo.OnDeselect -= ProxyTwoUnconfirmCharacter;

        playerInputProxy.Clear();

        Destroy(proxyOne.gameObject);
        Destroy(proxyTwo.gameObject);

        proxyOne = null;
        proxyTwo = null;
    }

    public ColorBlock GetColorBlock(int index)
    {
        return colorBlocks[index];
    }
}