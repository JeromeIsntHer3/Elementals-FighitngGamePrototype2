using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterContainerUI : MonoBehaviour
{
    public string pb_CharacterName;
    public CharacterInfo pb_Info;
    [SerializeField] MenuSceneManager manager;
    [SerializeField] Image indicator1, indicator2;

    Button containerButton;
    ColorBlock originalBlock;
    int selectedCount;

    Dictionary<int, Image> indicator = new();

    void Start()
    {
        containerButton = GetComponent<Button>();
        originalBlock = containerButton.colors;
        indicator.Add(0, indicator1);
        indicator.Add(1, indicator2);
    }

    public void Select(ColorBlock color, int index)
    {
        if(selectedCount == 0)
        {
            containerButton.colors = color;
            indicator[index].gameObject.SetActive(true);
        }
        else if(selectedCount == 1)
        {
            containerButton.colors = new ColorBlock()
            {
                colorMultiplier = 3,
                normalColor = Color.magenta
            };
            indicator[0].gameObject.SetActive(true);
            indicator[1].gameObject.SetActive(true);
        }

        selectedCount++;
    }

    public void Deselect(int index)
    {
        if(selectedCount == 1)
        {
            containerButton.colors = originalBlock;
            foreach(var ind in indicator.Values)
            {
                ind.gameObject.SetActive(false);
            }
        }
        else if(selectedCount == 2)
        {
            if(index == 0)
            {
                containerButton.colors = MenuSceneManager.Instance.GetColorBlock(1);
                indicator[0].gameObject.SetActive(false);
            }
            else
            {
                containerButton.colors = MenuSceneManager.Instance.GetColorBlock(0);
                indicator[1].gameObject.SetActive(false);
            }
        }
        
        selectedCount--;
        if(selectedCount < 0) selectedCount = 0;
    }
}