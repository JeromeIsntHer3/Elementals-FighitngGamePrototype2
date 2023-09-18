using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : SelectionUI
{
    [field: SerializeField] public Slider MasterSlider { get; set; }
    [field: SerializeField] public Slider SfxSlider {get; set;}
    [field: SerializeField] public Slider AmbienceSlider {get; set;}
    [field: SerializeField] public Slider MusicSlider {get; set;}

    void Start()
    {
        MasterSlider.onValueChanged.AddListener(MasterSliderChanged);
        SfxSlider.onValueChanged.AddListener(SFXSliderChanged);
        AmbienceSlider.onValueChanged.AddListener(AmbienceSliderChanged);
        MusicSlider.onValueChanged.AddListener(MusicSliderChanged);
    }

    void MasterSliderChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value / 100);
    }

    void SFXSliderChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value / 100);
    }

    void AmbienceSliderChanged(float value)
    {
        AudioManager.Instance.SetAmbienceVolume(value / 100);
    }
    void MusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value / 100);
    }

    public override void OnSelect()
    {
        base.OnSelect();
        UserInterfaceUtils.Instance.SelectNew(MasterSlider.gameObject, UIManager.Instance.MEventSystem, 
            UIManager.Instance.SettingsUI.gameObject);

        Debug.Log(UIManager.Instance.SettingsUI.gameObject);
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        UserInterfaceUtils.Instance.SelectNew(null, UIManager.Instance.MEventSystem,
            null);
    }
}