using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMRockProjectile : BaseProjectile
{
    [SerializeField] Collider2D rockCol;
    [SerializeField] Collider2D explosionCol;

    Animator anim;

    void OnEnable()
    {
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }

    public CMRockProjectile SetupProjectile(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan
        , float dist)
    {
        InitProjectile(owner, data, lifespan, false);
        anim = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        rb.velocity = dir.normalized * speed;
        Invoke(nameof(Expand), dist/speed);
        return this;
    }

    void Expand()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Explode()
    {
        hitSomething = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.gravityScale = 0;

        gameObject.layer = LayerMask.NameToLayer("Projectile");
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Projectile");
        }
        anim.CrossFade("Rock_Projectile_Explode", 0, 0);
        Invoke(nameof(DestroyRock), .525f);
    }

    void DestroyRock()
    {
        Destroy(gameObject);
    }

    void OnTrigger(object sender, Collider2D col)
    {
        if (hitSomething) return;
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;
        if (BelongsToOwner(hurtbox)) return;

        hitSomething = true;

        Vector3 fxSpawnPoint = explosionCol.ClosestPoint(hurtbox.transform.position);
        EffectManager.Instance.SpawnHitSplash(fxSpawnPoint, owner.IsFacingLeft);

        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        rockCol.gameObject.layer = gameObject.layer;

        if (hurtbox.BoxOwner.IsGuarding)
        {
            if (owner.IsFacingLeft && hurtbox.BoxOwner.IsFacingLeft)
            {
                hurtbox.Hit(damageData);
                hurtbox.BoxOwner.OnBlockCanceled?.Invoke(this, EventArgs.Empty);
                return;
            }

            hurtbox.BlockHit(damageData);
            return;
        }
        hurtbox.Hit(damageData);
    }

    public void HitRock(Vector2 direction, float hitForce)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.AddForce(hitForce * direction, ForceMode2D.Impulse);
        rb.gravityScale = 1.5f;
        hitSomething = false;

        gameObject.layer = LayerMask.NameToLayer("Projectile");
        rockCol.gameObject.layer = gameObject.layer;
    }
}