using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : NewHitbox
{
    HitData data;

    public void SetHitData(float damageAmount, float force)
    {
        data.DamageValue = damageAmount;
        data.AttackForce = force;
        data.AttackingObject = OwnerObject.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerHurtbox hurtbox))
        {
            hurtbox.TransferHitData(data);
        }
    }
}

public class HitData
{
    public float DamageValue;
    public float AttackForce;
    public Transform AttackingObject;
}