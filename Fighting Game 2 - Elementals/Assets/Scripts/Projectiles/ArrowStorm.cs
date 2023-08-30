using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStorm : BaseProjectile
{
    public void SetupStorm(BaseCharacter owner, DamageData data)
    {
        InitProjectile(owner, data, 1.2f, owner.IsFacingLeft);
    }
}