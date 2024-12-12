using System;
using UnityEngine;

public class LRMovement : BaseCharacterMovement
{
    [SerializeField] float pushbackForce;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OptionPerformedDelegate += Slide;
        OptionPerformCond += CanSlide;

        OptionCanceledDelegate += SlideCancel;
        OptionCancelCond += StopSliding;

        OptionUpdate += Recovery;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OptionPerformedDelegate -= Slide;
        OptionPerformCond -= CanSlide;

        OptionCanceledDelegate -= SlideCancel;
        OptionCancelCond -= StopSliding;

        OptionUpdate -= Recovery;
    }

    void Slide()
    {
        Vector2 dir = isFacingLeft ? Vector2.left : Vector2.right;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        rb.AddForce(Mathf.Abs(rb.linearVelocity.x) * data.SlideMultiplier * dir, ForceMode2D.Impulse);
    }

    void SlideCancel()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x / 2, rb.linearVelocity.y);
    }

    void Recovery()
    {
        character.SetRecoveryDuration(.1f);
    }

    bool StopSliding()
    {
        return Mathf.Abs(rb.linearVelocity.x) < .4f;
    }

    bool CanSlide()
    {
        return Mathf.Abs(rb.linearVelocity.x) > 4;
    }
}