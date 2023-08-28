using UnityEngine;

public class Hitbox : GameBox
{
    protected DamageData DamageData;
    protected delegate void HitboxExecute(Hurtbox hurtbox);
    protected HitboxExecute HitSuccessful;
    protected HitboxExecute HitBlock;

    bool hit;

    public virtual void SetDamageData(DamageData data, BaseCharacter owner)
    {
        hit = false;
        this.owner = owner;
        DamageData = data;
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (hit) return;
        if (col.TryGetComponent(out Hurtbox hurtbox))
        {
            if (CheckHitSelf(hurtbox)) return;

            Vector3 spawnPoint = GetComponent<Collider2D>().ClosestPoint(hurtbox.transform.position);
            EffectManager.Instance.SpawnHitSplash(spawnPoint, owner.IsFacingLeft);
            hit = true;
            if (hurtbox.BoxOwner.IsGuarding)
            {
                if (FacingSameDirection(hurtbox))
                {
                    hurtbox.Hit(DamageData);
                    hurtbox.BoxOwner.OnBlockCanceled?.Invoke(this, System.EventArgs.Empty);
                    return;
                }

                hurtbox.BlockHit(DamageData);
                HitBlock(hurtbox);
                return;
            }
            hurtbox.Hit(DamageData);
            HitSuccessful(hurtbox);
        }
    }

    protected bool CheckHitSelf(Hurtbox hurtbox)
    {
        return hurtbox.BoxOwner.gameObject == owner.gameObject;
    }

    bool FacingSameDirection(Hurtbox box)
    {
        return box.BoxOwner.IsFacingLeft && owner.IsFacingLeft;
    }
}