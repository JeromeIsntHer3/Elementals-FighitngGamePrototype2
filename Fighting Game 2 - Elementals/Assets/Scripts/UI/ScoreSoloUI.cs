using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSoloUI : MonoBehaviour
{
    [SerializeField] Image fillImg;


    public void SetColor(Color color)
    {
        fillImg.color = color;
    }

    public void Show(bool state)
    {
        fillImg.gameObject.SetActive(state);
    }
}