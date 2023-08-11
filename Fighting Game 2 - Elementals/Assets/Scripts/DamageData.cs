using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageData
{
    public float Damage;
    public DamageType Type;
    public float VerticalKnockback;
    public float HorizontalKnockback;
    public Vector2 Direction;
}

public enum DamageType
{

}