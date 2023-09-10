public class Beam : BaseProjectile
{
    public void SetupBeam(BaseCharacter character, DamageData data, float deathTime, bool flipX)
    {
        base.InitProjectile(character, data, deathTime, flipX);
    }
}