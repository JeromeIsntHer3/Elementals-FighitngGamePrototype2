using System.Collections.Generic;
using UnityEngine;

public class BaseMenuUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] protected List<AnimatedUIElement> animatedElements = new();

    public List<AnimatedUIElement> AnimatedElements { get { return animatedElements; } }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool Shown()
    {
        return gameObject.activeInHierarchy;
    }

    void Awake()
    {
        Show();
        canvas = GetComponent<Canvas>();
        Hide();
    }

    public Canvas m_Canvas {  get { return canvas; } }
}