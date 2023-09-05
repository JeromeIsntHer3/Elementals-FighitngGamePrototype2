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