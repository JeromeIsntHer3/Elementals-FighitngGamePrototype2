using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButtonUI : MonoBehaviour, ISelectHandler
{
    MenuSceneManager manager;
    CharacterContainerUI characterContainer;
    Button button;
    int playerIndex;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void SetupButtonUI(MenuSceneManager m, CharacterContainerUI c, int index)
    {
        manager = m;
        characterContainer = c;
        playerIndex = index;
    }

    public void OnSelect(BaseEventData eventData)
    {
        manager.SelectCharacter(characterContainer, playerIndex);
    }

    public void OnClick()
    {
        manager.ConfirmCharacter(characterContainer, playerIndex);
    }
}