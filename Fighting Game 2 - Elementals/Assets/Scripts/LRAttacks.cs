using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRAttacks : BaseCharacterAttacks
{
    [Header("Ranger Prefabs")]
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] Beam ultimateBeamPrefab;
    [SerializeField] ArrowStorm stormPrefab;

    [Header("Ranger Attack Values")]
    [SerializeField] float arrowSpeed;
    [SerializeField] float multiArrowSpeed;

    [Header("Ranger Arrow Spawns")]
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

    public void ArrowStab()
    {
        if (!enhance) return;
        var arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, arrowSpeed, 5f);
        enhance = false;
    }

    public void ShootNormal()
    {
        var arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, arrowSpeed, 5f);
        if (!enhance) return;
        enhance = false;
        ShootNormalEnhance();
    }

    public void ShootNormalEnhance()
    {
        var arrow = Instantiate(arrowPrefab, 
            new Vector2(arrowSpawn.position.x, arrowSpawn.position.y + 0.1f), Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.TwoEnhanced), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, arrowSpeed, 5f);

        arrow = Instantiate(arrowPrefab,
            new Vector2(arrowSpawn.position.x, arrowSpawn.position.y - 0.1f), Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.TwoEnhanced), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, arrowSpeed, 5f);
    }

    public void MultiShot()
    {
        if (enhance)
        {
            MultiShotEnhance();
            enhance = false;
        }
        else
        {
            foreach (Transform spawn in multiArrowSpawns)
            {
                var arrow = Instantiate(arrowPrefab, spawn.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two),
                    character, IsFacingLeft, spawn.position - centre.position, multiArrowSpeed, 5f);
            }
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
        storm.SetupStorm(character, GetDamageData(AttackType.ThreeEnhanced));
    }

    public void ShootWhileJumping()
    {
        var arrow = Instantiate(arrowPrefab, jumpArrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, jumpArrowSpawn.position - centre.position, arrowSpeed, 5f);
    }

    public void UltimateBlast()
    {
        var beam = Instantiate(ultimateBeamPrefab, beamSpawn.position, Quaternion.identity);
        beam.SetupBeam(GetDamageData(AttackType.Ultimate),character, IsFacingLeft);
    }
}