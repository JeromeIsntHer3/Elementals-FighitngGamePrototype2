using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStorm : Hitbox
{
    public void SetupArrowSpawn(DamageData data, BaseCharacter owner)
    {
        base.SetDamageData(data, owner);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}