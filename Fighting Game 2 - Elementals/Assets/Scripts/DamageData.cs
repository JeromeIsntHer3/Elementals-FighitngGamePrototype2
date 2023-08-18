using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageData
{
    public DamageType DamageType;
    public float Damage;
    public float VerticalKnockback;
    public float HorizontalKnockback;
    public Vector2 KnockbackDirection;
    public float StunDuration;
    public BaseCharacter Source;
}

public enum DamageType
{

}