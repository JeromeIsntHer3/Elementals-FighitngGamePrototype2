using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundProjectile : MonoBehaviour
{
    float deathTime;
    DamageData damageData;
    BaseCharacter thisOwner;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public GroundProjectile SetupProjectile(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan)
    {
        rb = GetComponent<Rigidbody2D>();
        damageData = data;
        thisOwner = owner;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = flipX;
        rb.velocity = dir.normalized * speed;
        deathTime = Time.time + lifespan;

        Invoke(nameof(Expand), .3f);

        return this;
    }

    void Expand()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}