using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerMovement : BaseCharacterMovement
{
    [SerializeField] float pushbackForce;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        character.OnUltimate += OnUltimate;

        OptionPerformedDelegate += Slide;
        OptionPerformCond += CanSlide;

        OptionCanceledDelegate += SlideCancel;
        OptionCancelCond += StopSliding;

        OptionUpdate += Recovery;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        character.OnUltimate -= OnUltimate;

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
        return Mathf.Abs(rb.velocity.x) < 2;
    }

    bool CanSlide()
    {
        return Mathf.Abs(rb.velocity.x) < 4;
    }

    void OnUltimate(object sender, EventArgs e)
    {
        if (!character.Recovered()) return;
        Invoke(nameof(UltimatePushBack), character.GetDuration(AnimationType.Ultimate) / 2);
    }

    void UltimatePushBack()
    {
        Vector2 dir = isFacingLeft ? Vector2.right : Vector2.left;
        rb.AddForce(dir * pushbackForce, ForceMode2D.Impulse);
    }
}