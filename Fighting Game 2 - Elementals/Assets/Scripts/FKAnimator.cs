public class FKAnimator : BaseCharacterAnimator
{
    FKAttacks attacks;

    protected override void OnEnable()
    {
        base.OnEnable();

        attacks = GetComponent<FKAttacks>();
        attacks.OnOptionStateChanged += OptionState;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        attacks.OnOptionStateChanged -= OptionState;
    }

    void OptionState(object sender, bool state)
    {
        spriteRenderer.material.SetFloat("_OutlineEnabled", state ? 1 : 0);
    }
}