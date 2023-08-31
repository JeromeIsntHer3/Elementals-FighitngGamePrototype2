using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterSplash : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void SetSplash(float activeTime, int color)
    {
        Invoke(nameof(DestroyDelay), activeTime);

        switch (color)
        {
            case 0:
                anim.CrossFade("BlueMeter",0,0);
                break;
            case 1:
                anim.CrossFade("YellowMeter", 0, 0);
                break;
            default: 
                break;
        }
    }

    void DestroyDelay()
    {
        Destroy(gameObject);
    }
}