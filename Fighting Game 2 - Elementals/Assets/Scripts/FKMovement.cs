using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKMovement : BaseCharacterMovement
{
    void Start()
    {
        OptionPerformCond = SomeBool;
    }

    bool SomeBool()
    {
        Debug.Log("SomeBool");
        return false;
    }
}