using UnityEngine;

public class Hurtbox : GameBox
{
    public void Hit(DamageData damageData)
    {
        damageData.KnockbackDirection = owner.transform.position - damageData.Source.transform.position;
        owner.GetComponent<BaseCharacterHealth>().Damage(damageData);
    }

    public void BlockHit(DamageData damageData)
    {
        damageData.KnockbackDirection = owner.transform.position - damageData.Source.transform.position;

        DamageData dd = damageData;
        if (owner.DefenseBroken && dd.Enhanced)
        {
            owner.GetComponent<BaseCharacterHealth>().Damage(damageData);
        }
        else
        {
            dd.Damage *= (100f - owner.DamageReduction) / 100;
            Debug.Log(owner.DamageReduction);
            owner.OnBlockHit?.Invoke(this, damageData);
        }
    }
}