using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenuUI : MonoBehaviour
{
    Canvas canvas;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        Hide();
    }

    public Canvas m_Canvas {  get { return canvas; } }
}