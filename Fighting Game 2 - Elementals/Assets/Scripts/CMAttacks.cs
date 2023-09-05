using UnityEngine;

public class CMAttacks : BaseCharacterAttacks
{
    [SerializeField] CMRockProjectile rockPrefab;
    [SerializeField] CMCrystalProjectile crystalPrefab;

    [SerializeField] Transform rockSpawn;
    [SerializeField] Transform crystalSpawn;

    [SerializeField] float rockSpeed;
    [SerializeField] float rockDist;
    [SerializeField] float crystalSpeed;
    [SerializeField] float crystalLifetime;

    [SerializeField] Hitbox hammerHitbox;
    [SerializeField] float hitForce;
    [SerializeField] float deflectDuration;

    CMRockProjectile gp_Rock;
    CMCrystalProjectile staticCrystal;
    float deflectTilTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        hammerHitbox.OnHitOther += HammerHit;
        hammerHitbox.OnHitOther += ReflectProjectile;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        hammerHitbox.OnHitOther -= HammerHit;
        hammerHitbox.OnHitOther -= ReflectProjectile;
    }

    void Update()
    {
        if (!hammerHitbox.CanDeflect) return;
        if (Time.time < deflectTilTime) return;
        hammerHitbox.SetDeflectState(false);
    }

    public void SpawnRock()
    {
        if(gp_Rock != null) Destroy(gp_Rock.gameObject);
        var rock = Instantiate(rockPrefab, rockSpawn.position, Quaternion.identity);
        gp_Rock = rock.GetComponent<CMRockProjectile>().SetupProjectile(GetDamageData(AttackType.Two), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, rockSpeed, 10f, rockDist);
    }

    public void SpawnCrystal()
    {
        var crystal = Instantiate(crystalPrefab, crystalSpawn.position, Quaternion.identity);
        crystal.SetupCrystal(character, GetDamageData(AttackType.Three), 0, 
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, crystalSpeed, enhance, true);

        staticCrystal = Instantiate(crystalPrefab, crystalSpawn.position, Quaternion.identity);
        Invoke(nameof(DestroyCrystal), 0.25f);

        if (enhance) enhance = false;
    }

    void DestroyCrystal()
    {
        if (!staticCrystal) return;
        Destroy(staticCrystal.gameObject);
    }

    void HammerHit(object sender, Collider2D col)
    {
        if (currentAttackType != AttackType.One) return;
        if (!enhance) return;
        enhance = false;

        if (!col.transform.root.TryGetComponent(out CMRockProjectile rock)) return;
        rock.HitRock(IsFacingLeft ? Vector2.left : Vector2.right, hitForce);
    }

    void ReflectProjectile(object sender, Collider2D col)
    {
        if (currentAttackType != AttackType.Two) return;
        if (!enhance) return;
        enhance = false;

        hammerHitbox.SetDeflectState(true);
        deflectTilTime = Time.time + deflectDuration;
    }
}