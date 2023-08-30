using UnityEngine;

public class Hurtbox : GameBox
{
    public void Hit(DamageData damageData)
    {
        damageData.KnockbackDirection = owner.transform.position - damageData.Source.transform.position;
        owner.OnHit?.Invoke(this, damageData);
    }

    public void BlockHit(DamageData damageData)
    {
        damageData.KnockbackDirection = owner.transform.position - damageData.Source.transform.position;

        DamageData dd = damageData;
        if (owner.DefenseBroken && dd.Enhanced)
        {
            owner.OnHit?.Invoke(this, damageData);
        }
        else
        {
            dd.Damage *= (100f - owner.DamageReduction) / 100;
            owner.OnBlockHit?.Invoke(this, damageData);
        }
    }
}