using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButtonUI : MonoBehaviour, ISelectHandler
{
    PlayableCharacter character;
    Button button;
    int playerIndex;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void SetupButtonUI(PlayableCharacter c, int index)
    {
        character = c;
        playerIndex = index;
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.OnSelectCharacter?.Invoke(this,
            new GameManager.OnSelectCharacterArgs
            {
                Character = character,
                PlayerIndex = playerIndex
            });
    }

    public void OnClick()
    {
        GameManager.OnConfirmCharacter?.Invoke(this,
            new GameManager.OnSelectCharacterArgs
            {
                Character = character,
                PlayerIndex = playerIndex
            });
    }
}