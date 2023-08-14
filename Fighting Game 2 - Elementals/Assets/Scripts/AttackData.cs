using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackData
{
    public AnimationType AnimationType;
    public DamageType DamageType;
    public float Damage;
    public float HorizontalKnockback;
    public float VerticalKnockback;
    public float StunDuration;
}