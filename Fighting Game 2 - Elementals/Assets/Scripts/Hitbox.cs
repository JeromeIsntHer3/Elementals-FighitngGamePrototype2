using UnityEngine;

public class Hitbox : GameBox
{
    AttackData data;
    DamageData damageData;

    //Create New DamageData on Input and not on trigger

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Hurtbox hurtbox))
        {
            if (hurtbox.CheckHitOwner(owner)) return;
            Vector2 attackDir = hurtbox.transform.root.position - owner.transform.position;
            damageData.Direction = attackDir;
            hurtbox.Hit(damageData);
        }
    }

    public void SetAttackData(AttackData data)
    {
        this.data = data;
        damageData = new DamageData
        {
            Damage = data.Damage,
            HorizontalKnockback = data.horizontalKnockback,
            VerticalKnockback = data.verticalKnockback,
            Type = data.DamageType
        };
    }
}