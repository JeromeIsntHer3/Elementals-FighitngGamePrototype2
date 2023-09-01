using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseMenuUI
{
    [SerializeField] MenuSceneManager manager;
    [SerializeField] Button playButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button quitButton;

    public Button PlayButton { get { return playButton; } }
    public Button SettingButton { get { return settingButton; } }
    public Button QuitButton { get { return quitButton; } }

    void Start()
    {
        playButton.onClick.AddListener(PlayPress);
        settingButton.onClick.AddListener(SettingsPress);
        quitButton.onClick.AddListener(QuitPress);
    }

    void PlayPress()
    {
        if (GameManager.GameState != GameState.Menu) return;
        MenuSceneManager.Instance.CharacterSelect.Show();
        MenuSceneManager.OnGoToCharacterSelect?.Invoke(this, System.EventArgs.Empty);
    }

    void SettingsPress()
    {

    }

    void QuitPress()
    {

    }
}