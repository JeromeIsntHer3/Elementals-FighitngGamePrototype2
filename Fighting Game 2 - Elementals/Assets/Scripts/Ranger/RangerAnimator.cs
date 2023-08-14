using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAnimator : BaseCharacterAnimator
{
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Awake()
    {
        base.Awake();
        OptionPerformCondition = SlideCondition;
    }

    bool SlideCondition()
    {
        return Mathf.Abs(rb.velocity.x) < 4;
    }
}