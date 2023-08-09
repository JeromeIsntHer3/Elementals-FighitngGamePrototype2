using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttacks : BaseCharacterAttacks
{
    [SerializeField] Beam ultimateBeamPrefab;
    [SerializeField] Transform arrowSpawn;
    [SerializeField] Transform jumpArrowSpawn;
    [SerializeField] Transform beamSpawn;
    [SerializeField] float arrowForce;
    [SerializeField] float multiArrowForce;
    [SerializeField] float arrowLifespan;
    [SerializeField] List<Transform> multiArrowSpawns = new();

    delegate void DelayShoot();
    DelayShoot DelayShootDelegate;

    public override void OnEnable()
    {
        base.OnEnable();
        Attack1 += ArrowStab;
        //Attack2 += ShootArrow;
        //Attack3 += ShootMultiArrow;
        //JumpAttack += ShootAirArrow;
        Ultimate += OnUltimate;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Attack1 -= ArrowStab;
        //Attack2 -= ShootArrow;
        //Attack3 -= ShootMultiArrow;
        //JumpAttack -= ShootAirArrow;
        Ultimate -= OnUltimate;
    }

    void ArrowStab()
    {
        SetRecoveryDuration(GetDuration(AnimationType.Attack1));
    }

    void ShootArrow()
    {
        SetRecoveryDuration(GetDuration(AnimationType.Attack2));
        DelayShootDelegate = ShootNormal;
        StartCoroutine(ShootAfterDelay(GetDuration(AnimationType.Attack2)/2));
    }

    void ShootMultiArrow()
    {
        SetRecoveryDuration(GetDuration(AnimationType.Attack3));
        DelayShootDelegate = ShootAirMulti;
        StartCoroutine(ShootAfterDelay(GetDuration(AnimationType.Attack3)/2));
    }

    void ShootAirArrow()
    {
        SetRecoveryDuration(GetDuration(AnimationType.JumpAttack));
    }

    void OnUltimate()
    {
        SetRecoveryDuration(GetDuration(AnimationType.Ultimate));
    }

    public void ShootAirMulti()
    {
        foreach (Transform spawn in multiArrowSpawns)
        {
            Vector2 dir = spawn.position - centre.position;
            var arrow = Instantiate(prefab, spawn.position, Quaternion.identity);
            arrow.GetComponent<Arrow>().Spawn(new Arrow.ArrowData
            {
                FlipSprite = IsFacingLeft,
                FlightDirection = dir,
                FlightSpeed = multiArrowForce,
                Lifespan = arrowLifespan
            });
        }
    }

    public void ShootNormal()
    {
        var arrow = Instantiate(prefab, arrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().Spawn(new Arrow.ArrowData
        {
            FlipSprite = IsFacingLeft,
            FlightDirection = IsFacingLeft ? Vector3.left : Vector3.right,
            FlightSpeed = arrowForce,
            Lifespan = arrowLifespan
        });
    }

    public void ShootWhileJumping()
    {
        var arrow = Instantiate(prefab, jumpArrowSpawn.position, Quaternion.identity);
        Vector2 dir = jumpArrowSpawn.position - centre.position;
        arrow.GetComponent<Arrow>().Spawn(new Arrow.ArrowData
        {
            FlipSprite = IsFacingLeft,
            FlightDirection = dir,
            FlightSpeed = arrowForce,
            Lifespan = arrowLifespan
        });
    }

    public void UltimateBlast()
    {
        var beam = Instantiate(ultimateBeamPrefab, beamSpawn.position, Quaternion.identity);
        beam.Spawn(new Beam.BeamData
        {
            FlipSprite = IsFacingLeft,
            Lifespan = .333f
        });
    }

    IEnumerator ShootAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);
        DelayShootDelegate?.Invoke();
    }
}