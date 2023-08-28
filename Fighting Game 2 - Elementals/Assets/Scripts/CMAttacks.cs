using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMAttacks : BaseCharacterAttacks
{
    [SerializeField] CMRockProjectile rockPrefab;
    [SerializeField] CMCrystalProjectile crystalPrefab;

    [SerializeField] Transform rockSpawn;
    [SerializeField] Transform crystalSpawn;

    [SerializeField] float rockSpeed;
    [SerializeField] float crystalSpeed;
    [SerializeField] float crystalLifetime;

    CMRockProjectile gp_Rock;
    CMCrystalProjectile staticCrystal;


    public void SpawnRock()
    {
        if(gp_Rock != null) Destroy(gp_Rock.gameObject);
        var rock = Instantiate(rockPrefab, rockSpawn.position, Quaternion.identity);
        gp_Rock = rock.GetComponent<CMRockProjectile>().SetupProjectile(GetDamageData(AttackType.Two), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, rockSpeed, 10f);
    }

    public void SpawnCrystal()
    {
        var crystal = Instantiate(crystalPrefab, crystalSpawn.position, Quaternion.identity);
        crystal.SetupCrystal(character, GetDamageData(AttackType.Three), 0, 
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, crystalSpeed);

        staticCrystal = Instantiate(crystalPrefab, crystalSpawn.position, Quaternion.identity);
        Invoke(nameof(DestroyCrystal), 0.25f);
    }

    void DestroyCrystal()
    {
        if (!staticCrystal) return;
        Destroy(staticCrystal.gameObject);
    }
}