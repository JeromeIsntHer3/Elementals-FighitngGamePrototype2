using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBAnimator : BaseCharacterAnimator
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