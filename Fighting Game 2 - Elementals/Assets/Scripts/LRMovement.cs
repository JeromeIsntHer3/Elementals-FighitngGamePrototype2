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
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        rb.AddForce(Mathf.Abs(rb.velocity.x) * data.SlideMultiplier * dir, ForceMode2D.Impulse);
    }

    void SlideCancel()
    {
        rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y);
    }

    void Recovery()
    {
        character.SetRecoveryDuration(.1f);
    }

    bool StopSliding()
    {
        return Mathf.Abs(rb.velocity.x) < .4f;
    }

    bool CanSlide()
    {
        return Mathf.Abs(rb.velocity.x) > 4;
    }
}