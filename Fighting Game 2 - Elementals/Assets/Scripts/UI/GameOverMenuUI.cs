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
    readonly bool[] playerSelected = new bool[2];


    void Awake()
    {
        playerSelected[0] = false;
        playerSelected[1] = false;
        playersGameOver[0].RematchButton.onClick.AddListener(delegate { OnRematchButtonPress(0); });
        playersGameOver[0].ToCharacterSelectButton.onClick.AddListener(OnCharacterSelectPress);
        playersGameOver[0].QuitButton.onClick.AddListener(OnQuitButtonPress);
        playersGameOver[1].RematchButton.onClick.AddListener(delegate { OnRematchButtonPress(1); });
        playersGameOver[1].ToCharacterSelectButton.onClick.AddListener(OnCharacterSelectPress);
        playersGameOver[1].QuitButton.onClick.AddListener(OnQuitButtonPress);
    }

    void OnEnable()
    {
        GameManager.OnGameRematch += OnGameRematch;
    }

    void OnDisable()
    {
        GameManager.OnGameRematch -= OnGameRematch;
    }

    void OnGameRematch(object sender, EventArgs args)
    {
        for(int i = 0; i < 2; i++)
        {
            playerSelected[i] = false;
            GameManager.Instance.GetPlayerProxy(i).OnDeselect -= OnBackPress;
            GameManager.Instance.GetPlayerProxy(i).SetEventSystemState(true);
        }
    }

    void OnRematchButtonPress(int index)
    {
        playerSelected[index] = true;

        GameManager.Instance.GetPlayerProxy(index).OnDeselect += OnBackPress;
        GameManager.Instance.GetPlayerProxy(index).SetEventSystemState(false);

        foreach (var check in playerSelected)
        {
            if (check == false) return;
        }

        GameManager.OnGameRematch?.Invoke(this, EventArgs.Empty);
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
        playerSelected[index] = false;
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