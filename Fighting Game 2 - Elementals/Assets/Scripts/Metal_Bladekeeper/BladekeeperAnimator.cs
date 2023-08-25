using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladekeeperAnimator : BaseCharacterAnimator
{
    void Start()
    {
        OptionPerformCondition = BackflipCondition;
    }

    bool BackflipCondition()
    {
        return true;
    }
}