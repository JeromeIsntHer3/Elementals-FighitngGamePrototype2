using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInterfaceUtils : MonoBehaviour
{
    public static UserInterfaceUtils Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SelectNew(Button button)
    {
        StartCoroutine(DelaySelect(button));
    }

    IEnumerator DelaySelect(Button button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        button.Select();
    }
}