using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerMovement : BaseCharacterMovement
{
    public override void OnEnable()
    {
        base.OnEnable();

        Debug.Log(cInput);

        cInput.OnUltimate += OnUltimate;

        OptionPerformedDelegate += Slide;
        OptionPerformCond += CanSlide;

        OptionCanceledDelegate += SlideCancel;
        OptionCancelCond += StopSliding;

        OptionUpdate += Recovery;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        cInput.OnUltimate -= OnUltimate;

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
        SetRecoveryDuration(.1f);
    }

    bool StopSliding()
    {
        return Mathf.Abs(HorizontalVelocity()) < 2;
    }

    bool CanSlide()
    {
        return Mathf.Abs(HorizontalVelocity()) < 4;
    }

    void OnUltimate(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        StartCoroutine(UltimatePushBack());
    }

    IEnumerator UltimatePushBack()
    {
        yield return new WaitForSeconds(GetDuration(AnimationType.Ultimate) / 2);
        Debug.Log("Pushback");
        Vector2 dir = isFacingLeft ? Vector2.right : Vector2.left;
        rb.AddForce(dir * 20f, ForceMode2D.Impulse);
    }
}