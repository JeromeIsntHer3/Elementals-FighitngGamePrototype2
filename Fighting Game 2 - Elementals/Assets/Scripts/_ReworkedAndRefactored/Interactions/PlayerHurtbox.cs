using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : NewHitbox
{
    public void TransferHitData(HitData hitData)
    {
        Vector2 knockbackDirection;

        knockbackDirection = (OwnerObject.transform.position - hitData.AttackingObject.transform.position).normalized;

        if(OwnerObject.TryGetComponent(out IKnockable knockable))
        {
            knockable.Knockback(knockbackDirection, hitData.AttackForce);
        }

        if(OwnerObject.TryGetComponent(out ICharacter character))
        {
            character.Damage(hitData.DamageValue);
        }
    }
}