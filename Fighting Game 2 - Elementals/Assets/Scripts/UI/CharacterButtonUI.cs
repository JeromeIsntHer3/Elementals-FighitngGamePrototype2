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
        MenuSceneManager.OnSelectCharacter?.Invoke(this,
            new MenuSceneManager.OnSelectCharacterArgs
            {
                Character = character,
                PlayerIndex = playerIndex
            });
    }

    public void OnClick()
    {
        MenuSceneManager.OnConfirmCharacter?.Invoke(this,
            new MenuSceneManager.OnSelectCharacterArgs
            {
                Character = character,
                PlayerIndex = playerIndex
            });
    }
}