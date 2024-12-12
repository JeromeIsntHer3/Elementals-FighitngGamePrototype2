using UnityEngine;

public class LRAnimator : BaseCharacterAnimator
{
    void Start()
    {
        OptionPerformCondition = SlideCondition;
        OptionCancelCondition = StopSliding;
    }

    bool SlideCondition()
    {
        return Mathf.Abs(rb.linearVelocity.x) > 4;
    }

    bool StopSliding()
    {
        return Mathf.Abs(rb.linearVelocity.x) < 2;
    }
}