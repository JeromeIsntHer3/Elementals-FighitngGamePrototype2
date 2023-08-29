using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BaseProjectile
{
    void OnEnable()
    {
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTrigger(object sender, Collider2D col)
    {
        if (hitSomething) return;
        if (col.TryGetComponent(out Hurtbox hurtbox))
        {
            if (BelongsToOwner(hurtbox)) return;

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

        if(col.TryGetComponent(out BaseProjectile projectile))
        {
            if (projectile.Owner == owner) return;
        }

        Destroy(gameObject);
    }

    public void SetupArrow(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan)
    {
        base.InitProjectile(owner, data, lifespan, flipX);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.AddForce(dir * speed,ForceMode2D.Impulse);
    }
}