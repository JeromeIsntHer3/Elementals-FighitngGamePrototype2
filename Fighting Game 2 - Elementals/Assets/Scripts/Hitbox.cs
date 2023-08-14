using UnityEngine;

public class Hitbox : GameBox, IHitbox
{
    DamageData damageData;

    public void SetAttackData(AttackData data)
    {
        damageData = new DamageData
        {
            Damage = data.Damage,
            HorizontalKnockback = data.HorizontalKnockback,
            VerticalKnockback = data.VerticalKnockback,
            Type = data.DamageType,
            StunDuration = data.StunDuration,
            Source = owner
        };
    }

    public DamageData Data()
    {
        return damageData;
    }
}