using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : BaseMenuUI
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button characterSelectButton;
    [SerializeField] Button quitToMenuButton;

    public Button ResumeButton { get { return resumeButton; } }
    public Button SettingsButton { get { return settingsButton; } }
    public Button CharacterSelectButton { get { return characterSelectButton; } }
    public Button QuitToMenuButton { get { return quitToMenuButton; } }

    void Start()
    {
        
    }
}