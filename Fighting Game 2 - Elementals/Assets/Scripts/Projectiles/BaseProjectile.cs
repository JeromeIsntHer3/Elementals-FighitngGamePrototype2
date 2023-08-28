using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected BaseCharacter owner;
    protected DamageData damageData;
    protected bool hitSomething = false;

    float activeTime;

    protected EventHandler<Collider2D> OnTriggerEvent;
    protected EventHandler<Collision2D> OnCollisionEvent;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEvent?.Invoke(this, collision);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEvent?.Invoke(this, collision);
    }

    public virtual void InitProjectile(BaseCharacter character, DamageData data, float time, bool flipX)
    {
        owner = character;
        damageData = data;
        activeTime = time;
        sr.flipX = flipX;

        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!sr) sr = GetComponent<SpriteRenderer>();
        hitSomething = false;

        if (activeTime > 0) Invoke(nameof(DestroyProjectile), activeTime);
    }

    protected virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    protected bool CheckHitOwner(Hurtbox hurtbox)
    {
        return hurtbox.BoxOwner.gameObject == owner.gameObject;
    }
}