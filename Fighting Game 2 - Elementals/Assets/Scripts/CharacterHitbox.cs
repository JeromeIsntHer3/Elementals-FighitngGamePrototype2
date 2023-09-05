public class CharacterHitbox : Hitbox
{
    void OnEnable()
    {
        HitSuccessful = HitSuccess;
        HitBlock = HitFailed;
    }

    void OnDisable()
    {
        HitSuccessful = null;
        HitBlock = null;
    }

    void HitSuccess(Hurtbox hb)
    {
        owner.OnHitEnemy?.Invoke(this, hb.BoxOwner);
    }

    void HitFailed(Hurtbox hb)
    {
        owner.OnHitBlocked?.Invoke(this, hb.BoxOwner);
    }
}