using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] HitSplash hitSplash;
    [SerializeField] MeterSplash meterSplash;

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

    public MeterSplash SpawnMeterSplash(Vector2 position, int colorCode)
    {
        var splash = Instantiate(meterSplash, position, Quaternion.identity);
        splash.SetSplash(.35f, colorCode);
        return splash;
    }
}