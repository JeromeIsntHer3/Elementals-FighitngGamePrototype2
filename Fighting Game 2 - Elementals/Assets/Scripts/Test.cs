using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class Test : MonoBehaviour
{
    [SerializeField] PauseMenuUI pauseUI;
    [SerializeField] MultiplayerEventSystem system;
 
    void Start()
    {
        Invoke(nameof(DelayedStart), 1f);
    }

    void DelayedStart()
    {
        pauseUI.Show();
        system.playerRoot = pauseUI.gameObject;
        system.SetSelectedGameObject(null);
        StartCoroutine(Utils.DelayEndFrame(()=>
        {
            system.SetSelectedGameObject(pauseUI.ResumeButton.gameObject);
        }));
    }
}