using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAnimator : BaseCharacterAnimator
{
    void Start()
    {
        OptionPerformCondition = SlideCondition;
        OptionCancelCondition = StopSliding;
    }

    bool SlideCondition()
    {
        return Mathf.Abs(rb.velocity.x) > 4;
    }

    bool StopSliding()
    {
        return Mathf.Abs(rb.velocity.x) < 2;
    }
}