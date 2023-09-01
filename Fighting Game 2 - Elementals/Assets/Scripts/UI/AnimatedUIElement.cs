using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUIElement : MonoBehaviour
{
    public Vector2 AnimateToPos;
    [HideInInspector] public RectTransform Rect;
    [SerializeField] bool startsAtAnimatedPosition;

    public Vector2 OriginalPosition;
    

    void Awake()
    {
        Rect = GetComponent<RectTransform>();

        OriginalPosition = Rect.anchoredPosition;
        if (!startsAtAnimatedPosition) return;
        Rect.anchoredPosition = AnimateToPos;
    }
}