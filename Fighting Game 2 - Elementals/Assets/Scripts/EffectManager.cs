using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] HitSplash hitSplash;

    void Awake()
    {
        Instance = this;   
    }

    public HitSplash SpawnHitSplash(Vector3 position, bool flipX)
    {
        var splash = Instantiate(hitSplash, position, Quaternion.identity);
        splash.Setup(.5f, flipX);
        return splash;
    }
}