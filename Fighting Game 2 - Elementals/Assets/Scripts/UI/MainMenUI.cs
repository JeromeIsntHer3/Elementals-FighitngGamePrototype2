using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseMenuUI
{
    [SerializeField] UIManager manager;
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
        UIManager.Instance.CharacterSelect.Show();
        UIManager.OnGoToCharacterSelect?.Invoke(this, System.EventArgs.Empty);
    }

    void SettingsPress()
    {

    }

    void QuitPress()
    {

    }
}