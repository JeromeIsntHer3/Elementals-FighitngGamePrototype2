using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContainerUI : MonoBehaviour
{
    [SerializeField] CharacterInfo characterInfo;
    [SerializeField] List<Image> indicators;

    Button containerButton;
    ColorBlock originalBlock;
    int selectedCount;

    public CharacterInfo Info { get { return characterInfo; } }


    void Start()
    {
        containerButton = GetComponent<Button>();
        originalBlock = containerButton.colors;
    }

    public void Select(ColorBlock color, int index)
    {
        if(selectedCount == 0)
        {
            containerButton.colors = color;
            indicators[index].gameObject.SetActive(true);
        }
        else if(selectedCount == 1)
        {
            containerButton.colors = new ColorBlock()
            {
                colorMultiplier = 3,
                normalColor = Color.magenta
            };
            indicators[0].gameObject.SetActive(true);
            indicators[1].gameObject.SetActive(true);
        }

        selectedCount++;
    }

    public void Deselect(int index)
    {
        if(selectedCount == 1)
        {
            containerButton.colors = originalBlock;
            foreach(var ind in indicators)
            {
                ind.gameObject.SetActive(false);
            }
        }
        else if(selectedCount == 2)
        {
            if(index == 0)
            {
                indicators[0].gameObject.SetActive(false);
                containerButton.colors = CharacterSelectMenuUI.Instance.PlayerTwoColorBlock;
            }
            else
            {
                indicators[1].gameObject.SetActive(false);
                containerButton.colors = CharacterSelectMenuUI.Instance.PlayerOneColorBlock;
            }
        }
        
        selectedCount--;
        if(selectedCount < 0) selectedCount = 0;
    }
}