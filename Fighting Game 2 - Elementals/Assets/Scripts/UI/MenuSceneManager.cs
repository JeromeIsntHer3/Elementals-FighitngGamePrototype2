using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TextMeshProUGUI playerOneCharacterName;
    [SerializeField] TextMeshProUGUI playerTwoCharacterName;
    public ColorBlock colorBlockOne;
    public ColorBlock colorBlockTwo;
    [SerializeField] Transform oneSpawn;
    [SerializeField] Transform twoSpawn;
    [SerializeField] TextMeshProUGUI readyText1;
    [SerializeField] TextMeshProUGUI readyText2;

    [Header("Multiplayer")]
    [SerializeField] PlayerInputManager pim;
    [SerializeField] EventSystem es;
    [SerializeField] GameObject playerPrefab;

    [Header("Character Containers")]
    [SerializeField] CharacterContainerUI ranger;
    [SerializeField] CharacterContainerUI knight;
    [SerializeField] CharacterContainerUI bladekeeper;
    [SerializeField] CharacterContainerUI mauler;

    #region Getter & Setters

    public CharacterContainerUI Ranger { get { return ranger; } }
    public CharacterContainerUI Knight { get { return knight; } }
    public CharacterContainerUI Bladekeeper { get { return bladekeeper; } }
    public CharacterContainerUI Mauler { get { return mauler; } }

    #endregion

    readonly Dictionary<int, CharacterContainerUI> selectedCharacter = new();
    readonly Dictionary<int, ColorBlock> indexColorBlock = new();
    readonly Dictionary<int, TextMeshProUGUI> indexName = new();
    readonly Dictionary<int, Transform> indexSpawn = new();
    readonly Dictionary<int, TextMeshProUGUI> indexReadyText = new();
    readonly Dictionary<int, CharacterSelectProxyUI> indexProxy = new();
    readonly Dictionary<int, CharacterContainerUI> confirmedCharacter = new();

    CharacterSelectProxyUI proxyOne;
    CharacterSelectProxyUI proxyTwo;

    void Start()
    {
        Instance = this;

        startButton.onClick.AddListener(ToCharacterSelect);
        settingsButton.onClick.AddListener(ToSettings);
        quitButton.onClick.AddListener(() => { Application.Quit(); });

        pim.playerPrefab = playerPrefab;

        indexColorBlock.Add(0, colorBlockOne);
        indexColorBlock.Add(1, colorBlockTwo);

        indexName.Add(0, playerOneCharacterName);
        indexName.Add(1, playerTwoCharacterName);

        indexSpawn.Add(0, oneSpawn);
        indexSpawn.Add(1, twoSpawn);

        indexReadyText.Add(0, readyText1);
        indexReadyText.Add(1, readyText1);

        confirmedCharacter.Add(0, null);
        confirmedCharacter.Add(1, null);
    }

    void ToCharacterSelect()
    {
        es.gameObject.SetActive(false);

        mainMenu.SetActive(false);
        characterSelect.SetActive(true);

        PlayerInput one = pim.JoinPlayer(0, default, "Keyboard");
        PlayerInput two = pim.JoinPlayer(1, default, "Controller");

        proxyOne = one.GetComponent<CharacterSelectProxyUI>().SetupProxy(this, one.playerIndex);
        proxyTwo = two.GetComponent<CharacterSelectProxyUI>().SetupProxy(this, two.playerIndex);

        indexProxy.Add(0, proxyOne);
        indexProxy.Add(1, proxyTwo);

        proxyOne.OnDeselect += ProxyOneDeselect;
        proxyTwo.OnDeselect += ProxyTwoDeselect;
    }

    void ToSettings()
    {

    }

    void BackToMainMenu()
    {
        proxyOne.OnDeselect -= ProxyOneDeselect;
        proxyTwo.OnDeselect -= ProxyTwoDeselect;

        indexProxy.Clear();

        Destroy(proxyOne.gameObject);
        Destroy(proxyTwo.gameObject);

        proxyOne = null;
        proxyTwo = null;

        es.gameObject.SetActive(true);

        mainMenu.SetActive(true);
        characterSelect.SetActive(false);
    }

    void ProxyOneDeselect(object sender, EventArgs args)
    {
        //Change return to else later on
        if (!confirmedCharacter[0]) return;
        readyText1.gameObject.SetActive(false);
        proxyOne.SetSelectionState(true);
        confirmedCharacter[0] = null;
        Debug.Log("Player One Deselected");
    }

    void ProxyTwoDeselect(object sender, EventArgs args)
    {
        if (!confirmedCharacter[1]) return;
        readyText2.gameObject.SetActive(false);
        proxyTwo.SetSelectionState(true);
        confirmedCharacter[1] = null;
        Debug.Log("Player Two Deselected");
    }

    void SelectNewCharacter(CharacterContainerUI c, int playerIndex)
    {
        //Set Values
        selectedCharacter[playerIndex] = c;
        selectedCharacter[playerIndex].Select(indexColorBlock[playerIndex], playerIndex);
        indexName[playerIndex].text = c.pb_CharacterName;
        //Spawn the character Display
        Transform spawnedT = Instantiate(c.pb_Info.Prefab, indexSpawn[playerIndex], false).transform;
        spawnedT.localPosition = c.pb_Info.RelativeSpawn;
    }

    public void SelectCharacter(CharacterContainerUI c, int playerIndex)
    {
        if(selectedCharacter.ContainsKey(playerIndex))
        {
            if (selectedCharacter[playerIndex])
            {
                selectedCharacter[playerIndex].Deselect(playerIndex);
                if (indexSpawn[playerIndex].GetChild(0) != null) Destroy(indexSpawn[playerIndex].GetChild(0).gameObject);
            }
            SelectNewCharacter(c, playerIndex);
        }
        else
        {
            selectedCharacter.Add(playerIndex, c);
            SelectNewCharacter(c, playerIndex);
        }
    }

    public void ConfirmCharacter(CharacterContainerUI c, int playerIndex)
    {
        Debug.Log($"Player {playerIndex + 1} confirmed {c.name}.");

        if (indexReadyText.ContainsKey(playerIndex))
        {
            indexReadyText[playerIndex].gameObject.SetActive(true);
            indexProxy[playerIndex].SetSelectionState(false);
            confirmedCharacter[playerIndex] = c;
        }
    }
}