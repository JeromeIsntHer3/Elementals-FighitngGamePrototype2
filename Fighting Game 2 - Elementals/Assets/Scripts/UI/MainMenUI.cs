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
        playButton.onClick.AddListener(PlayPressed);
        settingButton.onClick.AddListener(SettingsPressed);
        quitButton.onClick.AddListener(QuitPressed);
    }

    void PlayPressed()
    {
        if (GameManager.GameState != GameState.Menu) return;
        GameManager.OnToCharacterSelect?.Invoke(this, System.EventArgs.Empty);
    }

    void SettingsPressed()
    {

    }

    void QuitPressed()
    {

    }
}