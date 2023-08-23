using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterContainerUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] TextMeshProUGUI playerSelectText;
    [SerializeField] MenuSceneManager manager;

    Button containerButton;
    ColorBlock originalBlock;

    void Start()
    {
        containerButton = GetComponent<Button>();
        containerButton.onClick.AddListener(CharacterSelected);
        originalBlock = containerButton.colors;
    }

    void CharacterSelected()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        manager.SelectCharacterOne(this);
        ColorBlock block = manager.colorBlockOne;
        containerButton.colors = block;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        manager.SelectCharacterOne(null);
        containerButton.colors = originalBlock;
    }
}