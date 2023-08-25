using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMaulerAttacks : BaseCharacterAttacks
{
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject crystalPrefab;

    [SerializeField] Transform rockSpawn;
    [SerializeField] Transform crystalSpawn;

    [SerializeField] float rockSpeed;
    [SerializeField] float crystalSpeed;
    [SerializeField] float crystalLifetime;

    GroundProjectile gp_Rock;
    GameObject other;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnRock()
    {
        if(gp_Rock != null) Destroy(gp_Rock.gameObject);
        var rock = Instantiate(rockPrefab, rockSpawn.position, Quaternion.identity);
        gp_Rock = rock.GetComponent<GroundProjectile>().SetupProjectile(GetDamageData(AttackType.Two), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, rockSpeed, 5f);
    }

    public void SpawnCrystal()
    {
        var crystal = Instantiate(crystalPrefab, crystalSpawn.position, Quaternion.identity);
        crystal.GetComponent<CrystalProjectile>().SetupCrystal(crystalSpeed, IsFacingLeft ? Vector3.left : Vector3.right, crystalLifetime);
        other = Instantiate(crystalPrefab, crystalSpawn.position, Quaternion.identity);
        Invoke(nameof(DestroyDelay), 0.25f);
    }

    void DestroyDelay()
    {
        Destroy(other);
    }
}