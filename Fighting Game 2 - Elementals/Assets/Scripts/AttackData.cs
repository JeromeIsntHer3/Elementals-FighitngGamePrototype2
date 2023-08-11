using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackData
{
    public AnimationType AnimationType;
    public float Damage;
    public DamageType DamageType;
    public float horizontalKnockback;
    public float verticalKnockback;
}