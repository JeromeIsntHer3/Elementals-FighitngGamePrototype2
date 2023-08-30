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

    protected delegate void TriggerFunc(Collider2D col);
    protected TriggerFunc BeforeMainTrigger;
    protected TriggerFunc AfterMainTrigger;
    protected TriggerFunc BeforeGuardCheck;

    protected delegate void CollideFunc(Collision2D col);
    protected CollideFunc BeforeMainCollision;
    protected CollideFunc AfterMainCollision;

    public BaseCharacter Owner { get { return owner; } }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        BeforeMainTrigger?.Invoke(col);

        if (hitSomething) return;
        if (col.TryGetComponent(out Hurtbox hurtbox))
        {
            if (BelongsToOwner(hurtbox)) return;

            Vector3 spawnPoint = GetComponent<Collider2D>().ClosestPoint(hurtbox.transform.position);
            EffectManager.Instance.SpawnHitSplash(spawnPoint, owner.IsFacingLeft);
            hitSomething = true;

            BeforeGuardCheck?.Invoke(col);

            if (hurtbox.BoxOwner.IsGuarding)
            {
                if (owner.IsFacingLeft && hurtbox.BoxOwner.IsFacingLeft)
                {
                    hurtbox.Hit(damageData);
                    owner.OnHitEnemy?.Invoke(this, hurtbox.BoxOwner);
                    hurtbox.BoxOwner.OnBlockCanceled?.Invoke(this, System.EventArgs.Empty);
                    return;
                }

                owner.OnHitBlocked?.Invoke(this, hurtbox.BoxOwner);
                hurtbox.BlockHit(damageData);
                return;
            }
            owner.OnHitEnemy?.Invoke(this, hurtbox.BoxOwner);
            hurtbox.Hit(damageData);
        }

        if (col.TryGetComponent(out BaseProjectile projectile))
        {
            if (projectile.Owner == owner) return;
        }

        AfterMainTrigger?.Invoke(col);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        BeforeMainCollision?.Invoke(col);

        AfterMainCollision?.Invoke(col);
    }

    public virtual void InitProjectile(BaseCharacter character, DamageData data, float deathTime, bool flipX)
    {
        owner = character;
        damageData = data;
        activeTime = deathTime;
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

    protected bool BelongsToOwner(GameBox box)
    {
        return box.BoxOwner.gameObject == owner.gameObject;
    }
}