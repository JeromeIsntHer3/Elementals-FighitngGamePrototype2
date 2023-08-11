using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtboxes : GameBoxes
{
    DamageData recentAttack;

    public bool CanBeHit(DamageData data)
    {
        if (recentAttack == null) return true;
        if (recentAttack == data) return false;
        return true;
    }

    public void SetAttack(DamageData data)
    {
        DamageData dData = data;
        recentAttack = dData;
    }
}