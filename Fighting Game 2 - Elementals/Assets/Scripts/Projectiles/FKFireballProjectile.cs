using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKFireballProjectile : BaseProjectile
{
    bool onHit;

    void OnEnable()
    {
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }

    void OnTrigger(object sender, Collider2D col)
    {
        if (hitSomething) return;
        if (col.TryGetComponent(out Hurtbox hurtbox))
        {
            if (CheckHitOwner(hurtbox)) return;

            Vector3 spawnPoint = GetComponent<Collider2D>().ClosestPoint(hurtbox.transform.position);
            EffectManager.Instance.SpawnHitSplash(spawnPoint, owner.IsFacingLeft);
            hitSomething = true;
            if (hurtbox.BoxOwner.IsGuarding)
            {
                if (owner.IsFacingLeft && hurtbox.BoxOwner.IsFacingLeft)
                {
                    hurtbox.Hit(damageData);
                    hurtbox.BoxOwner.OnBlockCanceled?.Invoke(this, System.EventArgs.Empty);
                    return;
                }

                hurtbox.BlockHit(damageData);
                return;
            }
            hurtbox.Hit(damageData);
        }
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