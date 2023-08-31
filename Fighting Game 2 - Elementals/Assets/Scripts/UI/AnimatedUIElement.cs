using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUIElement : MonoBehaviour
{
    public Vector2 AnimateToPos;
    [SerializeField] bool startsAtAnimatedPosition;

    public Vector2 OriginalPosition;
    RectTransform elementRect;

    void Start()
    {
        elementRect = GetComponent<RectTransform>();

        OriginalPosition = elementRect.anchoredPosition;
        if (!startsAtAnimatedPosition) return;
        elementRect.anchoredPosition = AnimateToPos;
    }

    public void AnimateInto()
    {
        elementRect.DOKill();
        elementRect.DOAnchorPos(OriginalPosition, GameManager.UIAnimationDuration);
    }

    public void AnimateFrom()
    {
        elementRect.DOKill();
        elementRect.DOAnchorPos(AnimateToPos, GameManager.UIAnimationDuration);
    }
}