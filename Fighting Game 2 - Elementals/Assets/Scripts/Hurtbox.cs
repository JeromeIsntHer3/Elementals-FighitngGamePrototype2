using UnityEngine;

public class Hurtbox : GameBox
{
    [SerializeField] Hurtboxes hurtboxGroup;

    void Start()
    {
        hurtboxGroup = transform.parent.GetComponent<Hurtboxes>();
    }

    public bool CheckHitOwner(BaseCharacter arrowOwner)
    {
        return arrowOwner.gameObject == owner.gameObject;
    }

    public void Hit(DamageData dData)
    {
        if (!hurtboxGroup.CanBeHit(dData)) return;
        hurtboxGroup.SetAttack(dData);
        owner.GetComponent<BaseCharacterHealth>().Damage(dData);
    }
}