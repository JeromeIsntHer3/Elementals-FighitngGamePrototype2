using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageData
{
    public float Damage;
    public float VerticalKnockback;
    public float HorizontalKnockback;
    public Vector2 KnockbackDirection;
    public float HitStunDuration;
    public float BlockStunDuration;
    public BaseCharacter Source;
    public bool Enhanced;
}