using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMRockProjectile : BaseProjectile
{
    Animator anim;

    void OnEnable()
    {
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }

    public CMRockProjectile SetupProjectile(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan)
    {
        InitProjectile(owner, data, lifespan, false);
        anim = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        rb.velocity = dir.normalized * speed;
        Invoke(nameof(Expand), .3f);
        return this;
    }

    void Expand()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Explode()
    {
        StartCoroutine(WaitFor());
    }

    void DestroyRock()
    {
        Destroy(gameObject);
    }

    void OnTrigger(object sender, Collider2D col)
    {
        if (hitSomething) return;
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;
        if (CheckHitOwner(hurtbox)) return;

        Vector3 fxSpawnPoint = GetComponent<Collider2D>().ClosestPoint(hurtbox.transform.position);
        EffectManager.Instance.SpawnHitSplash(fxSpawnPoint, owner.IsFacingLeft);

        hitSomething = true;
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

    IEnumerator WaitFor()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Projectile");
        }
        yield return new WaitForEndOfFrame();
        anim.CrossFade("Rock_Projectile_Explode", 0, 0);
        Invoke(nameof(DestroyRock), .525f);
    }
}