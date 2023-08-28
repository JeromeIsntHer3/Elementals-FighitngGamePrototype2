using UnityEngine;

public class Hurtbox : GameBox
{
    public void Hit(DamageData damageData)
    {
        DamageData damage = damageData;
        damageData.KnockbackDirection = owner.transform.position - damageData.Source.transform.position;
        owner.GetComponent<BaseCharacterHealth>().Damage(damage);
    }

    public void BlockHit(DamageData damageData)
    {
        damageData.KnockbackDirection = owner.transform.position - damageData.Source.transform.position;
        owner.OnBlockHit?.Invoke(this, damageData);
    }
}