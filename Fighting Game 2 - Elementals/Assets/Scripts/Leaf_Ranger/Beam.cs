using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arrow;

public class Beam : Hitbox
{
    SpriteRenderer sr;

    public void SetupBeam(DamageData data, BaseCharacter owner, bool flipX)
    {
        base.SetDamageData(data, owner);
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = flipX;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}