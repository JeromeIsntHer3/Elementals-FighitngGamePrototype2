using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UserInterfaceUtils : MonoBehaviour
{
    public static UserInterfaceUtils Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SelectNew(Button button, MultiplayerEventSystem mes, GameObject root)
    {
        StartCoroutine(DelaySelect(button, mes, root));
    }

    IEnumerator DelaySelect(Button button, MultiplayerEventSystem mes, GameObject root)
    {
        mes.SetSelectedGameObject(null);
        mes.playerRoot = root;
        yield return new WaitForEndOfFrame();
        mes.SetSelectedGameObject(button.gameObject);
    }
}