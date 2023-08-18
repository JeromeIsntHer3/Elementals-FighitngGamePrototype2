using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : Hitbox
{
    void Start()
    {
        HitSuccessful = (Hurtbox box) =>
        {
            box.Hit(DamageData);
        };

        HitBlock = (Hurtbox box) =>
        {
            box.BlockHit(DamageData);
        };
    }
}