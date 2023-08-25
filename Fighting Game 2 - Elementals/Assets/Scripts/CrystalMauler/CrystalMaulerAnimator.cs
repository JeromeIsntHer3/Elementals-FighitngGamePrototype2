using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMaulerAnimator : BaseCharacterAnimator
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