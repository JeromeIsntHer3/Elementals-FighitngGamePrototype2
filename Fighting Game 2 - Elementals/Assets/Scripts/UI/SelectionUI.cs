using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] List<SelectionModule> modules = new();
    [SerializeField] Image forwardIcon, backIcon;
    [SerializeField] TextMeshProUGUI currentSelectionText;

    void Start()
    {

    }

    void Update()
    {

    }

    void SetIconActive(bool active)
    {
        forwardIcon.gameObject.SetActive(active);
        backIcon.gameObject.SetActive(active);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetIconActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetIconActive(false);
    }
}

[Serializable]
public class SelectionModule
{
    public string SelectionName;
    public GameObject SelectionObject;
}