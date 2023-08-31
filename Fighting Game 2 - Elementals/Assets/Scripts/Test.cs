using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] RectTransform top, left, right, bottom;
    [SerializeField] Vector2 topPos,leftPos, rightPos, bottomPos;

    Vector2 topOgPos, leftOgPos, rightOgPos, bottomOgPos;

    void Start()
    {
        topOgPos = top.anchoredPosition;
        leftOgPos = left.anchoredPosition;
        rightOgPos = right.anchoredPosition;
        bottomOgPos = bottom.anchoredPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(top.DOAnchorPos(topPos,1));
        sequence.Join(bottom.DOAnchorPos(bottomPos, 1));
        sequence.Join(left.DOAnchorPos(leftPos, 1));
        sequence.Join(right.DOAnchorPos(rightPos, 1));

        sequence.OnComplete(() => { Debug.Log("Completed"); });
    }
}