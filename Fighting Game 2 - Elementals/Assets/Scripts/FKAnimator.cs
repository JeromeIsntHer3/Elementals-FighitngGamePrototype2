using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKAnimator : BaseCharacterAnimator
{
    FKAttacks attacks;

    public override void OnEnable()
    {
        base.OnEnable();

        attacks = GetComponent<FKAttacks>();
        attacks.OnOptionStateChanged += OptionState;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        attacks.OnOptionStateChanged -= OptionState;
    }

    void OptionState(object sender, bool state)
    {
        spriteRenderer.material.SetFloat("_OutlineEnabled", state ? 1 : 0);
    }
}