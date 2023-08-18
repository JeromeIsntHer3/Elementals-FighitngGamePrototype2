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
        owner.OnBlockHit?.Invoke(this, damageData);
    }
}