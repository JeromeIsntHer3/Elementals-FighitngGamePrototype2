using Unity.VisualScripting;
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
                return;
            }
            hurtbox.Hit(DamageData);
            Debug.Log($"{owner} hit {hurtbox.BoxOwner} with {this}");
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