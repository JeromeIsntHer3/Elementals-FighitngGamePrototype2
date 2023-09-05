using UnityEngine;

public class MBMovement : BaseCharacterMovement
{
    void Start()
    {
        OptionPerformCond = BackflipCondition;
        OptionPerformedDelegate = BackflipPerform;
    }

    void BackflipPerform()
    {
        Invoke(nameof(DelayJump), .08f);
    }

    void DelayJump()
    {
        rb.velocity = Vector2.zero;
        movement.x = 0;
        Vector2 dir = isFacingLeft ? new Vector2(.5f, 1) : new Vector2(-.5f, 1);
        rb.AddForce(.8f * data.DashForce * dir, ForceMode2D.Impulse);
    }

    bool BackflipCondition()
    {
        return true;
    }
}