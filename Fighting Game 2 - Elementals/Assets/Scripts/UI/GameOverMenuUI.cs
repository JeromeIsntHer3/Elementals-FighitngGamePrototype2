using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuUI : BaseMenuUI
{
    [Serializable]
    public class PlayerSideGameOver
    {
        public GameObject Side;
        public Button RematchButton;
        public Button ToCharacterSelectButton;
        public Button QuitButton;
    }

    [SerializeField] List<PlayerSideGameOver> playersGameOver;
    readonly Dictionary<int, bool> playerSelected = new();


    void Awake()
    {
        playerSelected.Add(0, false);
        playerSelected.Add(1, false);
        playersGameOver[0].RematchButton.onClick.AddListener(delegate { OnRematchButtonPress(0); });
        playersGameOver[0].ToCharacterSelectButton.onClick.AddListener(OnCharacterSelectPress);
        playersGameOver[0].QuitButton.onClick.AddListener(OnQuitButtonPress);
        playersGameOver[1].RematchButton.onClick.AddListener(delegate { OnRematchButtonPress(1); });
        playersGameOver[1].ToCharacterSelectButton.onClick.AddListener(OnCharacterSelectPress);
        playersGameOver[1].QuitButton.onClick.AddListener(OnQuitButtonPress);

        
    }

    void OnRematchButtonPress(int index)
    {
        playerSelected[index] = true;

        GameManager.Instance.GetPlayerProxy(index).OnDeselect += OnBackPress;
        GameManager.Instance.GetPlayerProxy(index).SetEventSystemState(false);

        foreach (var check in playerSelected.Values)
        {
            if (check == false) return;
        }

        
    }

    void OnCharacterSelectPress()
    {
        GameManager.OnToCharacterSelect?.Invoke(this, EventArgs.Empty);
    }

    void OnQuitButtonPress()
    {
        GameManager.OnToMenu?.Invoke(this, EventArgs.Empty);
    }

    void OnBackPress(object sender, int index)
    {
        GameManager.Instance.GetPlayerProxy(index).SetEventSystemState(true);
        GameManager.Instance.GetPlayerProxy(index).OnDeselect -= OnBackPress;
    }

    public GameOverUISide GetUISide(int index)
    {
        GameOverUISide sideMenu = new()
        {
            RootObject = playersGameOver[index].Side,
            SelectObject = playersGameOver[index].RematchButton.gameObject
        };
        return sideMenu;
    }
}

public class GameOverUISide
{
    public GameObject RootObject;
    public GameObject SelectObject;
}