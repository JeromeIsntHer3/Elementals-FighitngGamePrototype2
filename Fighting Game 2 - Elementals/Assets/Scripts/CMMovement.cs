using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMMovement : BaseCharacterMovement
{
    void Start()
    {
        OptionPerformCond = SomeBool;
    }

    bool SomeBool()
    {
        return false;
    }
}