using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttacks : BaseCharacterAttacks
{
    [Header("Ranger Prefabs")]
    [SerializeField] Beam ultimateBeamPrefab;

    [Header("Ranger Attack Values")]
    [SerializeField] float arrowForce;
    [SerializeField] float multiArrowForce;
    [SerializeField] float arrowLifespan;

    [Header("Ranger Arrow Spawns")]
    [SerializeField] Transform arrowSpawn;
    [SerializeField] Transform jumpArrowSpawn;
    [SerializeField] Transform beamSpawn;
    [SerializeField] List<Transform> multiArrowSpawns = new();

    public void ShootAirMulti()
    {
        foreach (Transform spawn in multiArrowSpawns)
        {
            Vector2 dir = spawn.position - centre.position;
            var arrow = Instantiate(prefab, spawn.position, Quaternion.identity);
            arrow.GetComponent<Arrow>().SpawnArrow(new Arrow.ArrowData
            {
                FlipSprite = IsFacingLeft,
                FlightDirection = dir,
                FlightSpeed = multiArrowForce,
                DeathTime = Time.time + arrowLifespan,
                Damage = attackData[AnimationType.Attack2].Damage,
                Source = this,
                VerticalKnockback = attackData[AnimationType.Attack2].VerticalKnockback,
                HorizontalKnockback = attackData[AnimationType.Attack2].HorizontalKnockback,
                StunDuration = attackData[AnimationType.Attack2].StunDuration,
            });
        }
    }

    public void ArrowStab()
    {

    }

    public void ShootNormal()
    {
        var arrow = Instantiate(prefab, arrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SpawnArrow(new Arrow.ArrowData
        {
            FlipSprite = IsFacingLeft,
            FlightDirection = IsFacingLeft ? Vector3.left : Vector3.right,
            FlightSpeed = arrowForce,
            DeathTime = Time.time + arrowLifespan,
            Damage = attackData[AnimationType.Attack2].Damage,
            Source = this,
            VerticalKnockback = attackData[AnimationType.Attack2].VerticalKnockback,
            HorizontalKnockback = attackData[AnimationType.Attack2].HorizontalKnockback,
            StunDuration = attackData[AnimationType.Attack2].StunDuration,
        });
    }

    public void ShootWhileJumping()
    {
        var arrow = Instantiate(prefab, jumpArrowSpawn.position, Quaternion.identity);
        Vector2 dir = jumpArrowSpawn.position - centre.position;
        arrow.GetComponent<Arrow>().SpawnArrow(new Arrow.ArrowData
        {
            FlipSprite = IsFacingLeft,
            FlightDirection = dir,
            FlightSpeed = arrowForce,
            DeathTime = Time.time + arrowLifespan,
            Damage = attackData[AnimationType.Attack2].Damage,
            Source = this,
            VerticalKnockback = attackData[AnimationType.Attack2].VerticalKnockback,
            HorizontalKnockback = attackData[AnimationType.Attack2].HorizontalKnockback,
            StunDuration = attackData[AnimationType.Attack2].StunDuration,
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
}