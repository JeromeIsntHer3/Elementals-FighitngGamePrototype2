using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBKnife : BaseProjectile
{
    bool trap;
    bool landed;
    Animator anim;

    void OnEnable()
    {
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }

    public void SetupKnife(BaseCharacter owner, DamageData data, float lifespan, bool flipX, 
        Vector2 direction, float speed, bool isTrap)
    {
        InitProjectile(owner, data, lifespan, false);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
        trap = isTrap;
        anim = GetComponent<Animator>();
        Invoke(nameof(Explode), lifespan / 2);
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y,rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Explode()
    {
        anim.CrossFade("KnifeExplode", 0, 0);
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

        if (trap)
        {
            if (landed) return;
            Vector3 landingPoint = GetComponent<Collider2D>().ClosestPoint(col.transform.position);
            anim.CrossFade("Knife_Land", 0, 0);
            transform.position = landingPoint;
            transform.eulerAngles = Vector3.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            landed = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}