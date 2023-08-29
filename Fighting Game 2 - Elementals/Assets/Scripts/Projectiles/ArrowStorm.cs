using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStorm : BaseProjectile
{
    void OnEnable()
    {
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }


    public void SetupStorm(BaseCharacter owner, DamageData data)
    {
        InitProjectile(owner, data, 1.2f, owner.IsFacingLeft);
    }

    void OnTrigger(object sender, Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;

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
}