using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKFireballProjectile : BaseProjectile
{
    bool onHit;

    void OnEnable()
    {
        AfterMainTrigger = AfterTrigger;
    }

    void OnDisable()
    {
        AfterMainTrigger = null;
    }

    void AfterTrigger(Collider2D col)
    {
        if (onHit) Destroy(gameObject);
    }

    public void SetupFireball(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan, bool removeOnHit = false)
    {
        base.InitProjectile(owner, data, lifespan, flipX);
        if (!rb) return;
        rb.velocity = dir * speed;
        onHit = removeOnHit;
    }
}