using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Characters")]
    [SerializeField] CharacterContainerUI ranger;
    [SerializeField] CharacterContainerUI knight;

    CharacterContainerUI selectedCharacterOne;
    CharacterContainerUI selectedCharacterTwo;

    [HideInInspector] public Color playerOneColor = Color.blue;
    [HideInInspector] public Color playerTwoColor = Color.red;

    void Start()
    {
        Instance = this;

        startButton.onClick.AddListener(ToCharacterSelect);
        settingsButton.onClick.AddListener(ToSettings);
        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    void ToCharacterSelect()
    {
        mainMenu.SetActive(false);
        characterSelect.SetActive(true);
        UserInterfaceUtils.Instance.SelectNew(ranger.GetComponent<Button>());
    }

    void ToSettings()
    {

    }

    public void SelectCharacterOne(CharacterContainerUI c)
    {

    }

    public void SelectCharacterTwo(CharacterContainerUI c)
    {

    }

    public void ConfirmCharacterOne(CharacterContainerUI c)
    {

    }

    public void ConfirmCharacterTwo(CharacterContainerUI c)
    {

    }
}