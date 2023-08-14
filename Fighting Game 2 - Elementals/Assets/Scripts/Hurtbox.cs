using UnityEngine;

public class Hurtbox : GameBox
{
    [SerializeField] Hurtboxes hurtboxGroup;

    void Start()
    {
        hurtboxGroup = transform.parent.GetComponent<Hurtboxes>();
    }

    void Hit(DamageData dData)
    {
        if (!hurtboxGroup.CanBeHit(dData)) return;
        hurtboxGroup.SetAttack(dData);
        owner.GetComponent<BaseCharacterHealth>().Damage(dData);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHitbox hitbox))
        {
            hitbox.Data().Direction = owner.transform.position - hitbox.Data().Source.transform.position;
            Hit(hitbox.Data());
        }
    }
}