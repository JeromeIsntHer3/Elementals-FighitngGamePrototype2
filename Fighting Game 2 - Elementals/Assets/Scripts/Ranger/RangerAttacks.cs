using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttacks : BaseCharacterAttacks
{
    [Header("Ranger Prefabs")]
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

    bool enhance;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Enhance1 = () => { enhance = true; };
        Enhance2 = () => { enhance = true; };
        Enhance3 = () => { enhance = true; };
        EnhanceJump = () => { enhance = true; };
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Enhance1 = null;
        Enhance2 = null;
        Enhance3 = null;
        EnhanceJump = null;
    }

    public void ArrowStab()
    {

    }

    public void ShootNormal()
    {
        if (enhance)
        {
            enhance = false;
            ShootNormalEnhance();
        }

        var arrow = Instantiate(prefab, arrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, arrowSpeed, 5f);
    }

    public void ShootNormalEnhance()
    {
        var arrow = Instantiate(prefab, 
            new Vector2(arrowSpawn.position.x, arrowSpawn.position.y + 0.1f), Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.TwoEnhanced), character,
            IsFacingLeft, IsFacingLeft ? Vector3.left : Vector3.right, arrowSpeed, 5f);

        arrow = Instantiate(prefab,
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
                var arrow = Instantiate(prefab, spawn.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two),
                    character, IsFacingLeft, spawn.position - centre.position, multiArrowSpeed, 5f);
            }
        }
    }

    void MultiShotEnhance()
    {
        var storm = Instantiate(stormPrefab, stormSpawn.position, Quaternion.identity);
        storm.SetupArrowSpawn(GetDamageData(AttackType.ThreeEnhanced), character);
    }

    public void ShootWhileJumping()
    {
        var arrow = Instantiate(prefab, jumpArrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, jumpArrowSpawn.position - centre.position, arrowSpeed, 5f);
    }

    public void UltimateBlast()
    {
        var beam = Instantiate(ultimateBeamPrefab, beamSpawn.position, Quaternion.identity);
        beam.SetupBeam(GetDamageData(AttackType.Ultimate),character, IsFacingLeft);
    }
}