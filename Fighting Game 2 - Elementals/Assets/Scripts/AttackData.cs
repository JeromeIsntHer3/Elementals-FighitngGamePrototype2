using System;

[Serializable]
public class AttackData
{
    public AttackType AttackType;
    public float Damage;
    public float HorizontalKnockback;
    public float VerticalKnockback;
    public float HitStunDuration = .2f;
    public float BlockStunDuration = .2f;
}