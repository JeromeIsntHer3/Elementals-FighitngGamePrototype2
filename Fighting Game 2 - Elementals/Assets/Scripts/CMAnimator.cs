using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMAnimator : BaseCharacterAnimator
{
    void Start()
    {
        OptionPerformCondition = COndition;
    }

    bool COndition()
    {
        return true;
    }
}