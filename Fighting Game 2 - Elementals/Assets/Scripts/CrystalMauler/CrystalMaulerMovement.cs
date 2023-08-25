using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMaulerMovement : BaseCharacterMovement
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