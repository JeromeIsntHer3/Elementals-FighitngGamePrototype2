using System.Collections.Generic;
using UnityEngine;

public class LRAttacks : BaseCharacterAttacks
{
    [Header("Prefabs")]
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] Beam ultimateBeamPrefab;
    [SerializeField] ArrowStorm stormPrefab;

    [Header("Arrow Values")]
    [SerializeField] float arrowSpeed;
    [SerializeField] float multiArrowSpeed;

    [Header("Arrow Spawns")]
    [SerializeField] Transform arrowSpawn;
    [SerializeField] Transform jumpArrowSpawn;
    [SerializeField] Transform beamSpawn;
    [SerializeField] Transform stormSpawn;
    [SerializeField] List<Transform> multiArrowSpawns = new();

    Vector2 stormDirection;

    protected override void OnEnable()
    {
        base.OnEnable();
        character.OnMovement += OnMovement;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        character.OnMovement -= OnMovement;
    }

    void OnMovement(object sender, Vector2 dir)
    {
        stormDirection = dir;
    }

    public void ArrowStabEnhance()
    {
        if (!enhance) return;
        CreateArrow(arrowSpawn.position, IsFacingLeft ? Vector2.left : Vector2.right, arrowSpeed, 5f, true);
        enhance = false;
    }

    public void ShootArrow()
    {
        CreateArrow(arrowSpawn.position, IsFacingLeft ? Vector2.left : Vector2.right, arrowSpeed, 5f);
        if (!enhance) return;
        enhance = false;
        CreateArrow(new Vector2(arrowSpawn.position.x, arrowSpawn.position.y + .1f), 
            IsFacingLeft ? Vector2.left : Vector2.right, arrowSpeed,
            5f, true);
        CreateArrow(new Vector2(arrowSpawn.position.x, arrowSpawn.position.y - .1f),
            IsFacingLeft ? Vector2.left : Vector2.right, arrowSpeed,
            5f, true);
    }

    public void MultiShot()
    {
        if (enhance)
        {
            MultiShotEnhance();
            enhance = false;
            return;
        }
        foreach (Transform spawn in multiArrowSpawns)
        {
            CreateArrow(spawn.position, spawn.position - centre.position, multiArrowSpeed, 5f);
        }
    }

    void MultiShotEnhance()
    {
        Vector2 spawnPos = stormSpawn.position;

        if(stormDirection.x > 0)
        {
            spawnPos = new Vector2(spawnPos.x + 3, spawnPos.y);
        }
        else if(stormDirection.x < 0)
        {
            spawnPos = new Vector2(spawnPos.x - 2, spawnPos.y);
        }

        var storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);
        DamageData stormData = GetDamageData(AttackType.ThreeEnhanced);
        stormData.Enhanced = true;
        storm.SetupStorm(character, stormData);
    }

    public void ShootWhileJumping()
    {
        CreateArrow(jumpArrowSpawn.position, jumpArrowSpawn.position - centre.position, arrowSpeed, 5f);
    }

    public void UltimateBlast()
    {
        var beam = Instantiate(ultimateBeamPrefab, beamSpawn.position, Quaternion.identity);
        beam.SetupBeam(character,GetDamageData(AttackType.Ultimate), 3, IsFacingLeft);
    }

    DamageData GetArrowDamageData()
    {
        return GetDamageData(AttackType.Projectile);
    }

    void CreateArrow(Vector2 position, Vector2 direction, float speed, float activeTime, bool enhance = false)
    {
        var arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
        DamageData arrowData = GetArrowDamageData();
        arrowData.Enhanced = false;
        arrow.SetupArrow(arrowData, character, false, direction, speed, activeTime);
    }
}