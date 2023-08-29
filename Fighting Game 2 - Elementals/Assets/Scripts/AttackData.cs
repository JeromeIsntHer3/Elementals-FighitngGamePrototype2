using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackData
{
    public AttackType AttackType;
    public float Damage;
    public float HorizontalKnockback;
    public float VerticalKnockback;
    public float StunDuration = .2f;
    public int MeterUsage = 50;
}