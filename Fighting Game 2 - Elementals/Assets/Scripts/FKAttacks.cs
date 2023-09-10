using System;
using UnityEngine;

public class FKAttacks : BaseCharacterAttacks
{
    [Header("Heat Meter")]
    [SerializeField] FKHeatBarUI heatBar;
    [SerializeField] float maxHeatValue;
    [SerializeField] float currentHeatValue;
    [SerializeField] float drainAmount;
    [SerializeField] float gainAmount;
    [SerializeField][Range(1, 2)] float heatDamageMultiplier;

    [Header("Attack 1 Enhance")]
    [SerializeField] FKFireballProjectile fireslashPrefab;
    [SerializeField] Transform slashSpawn;
    [SerializeField] float attack1Drain;

    [Header("Attack 2 Enhance")]
    [SerializeField] FKFireballProjectile firediscPrefab;
    [SerializeField] float discSpeed;
    [SerializeField] float attack2Drain;

    [Header("Attack 3 Enhance")]
    [SerializeField] FKFireballProjectile explosionPrefab;
    [SerializeField] Transform explosionSpawn;
    [SerializeField] float attack3Drain;

    bool option;

    public EventHandler<bool> OnOptionStateChanged;
    public EventHandler<float> OnHeatValueChanged;
    public EventHandler OnHeatBurnComplete;

    public float MaxHeatValue { get { return maxHeatValue; } }


    protected override void OnEnable()
    {
        base.OnEnable();

        character.OnOption += OnOptionPressed;
        character.OnAttackOne += OnAttack1;
        character.OnAttackTwo += OnAttack2;
        character.OnAttackThree += OnAttack3;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        character.OnOption -= OnOptionPressed;
        character.OnAttackOne -= OnAttack1;
        character.OnAttackTwo -= OnAttack2;
        character.OnAttackThree -= OnAttack3;
    }

    void OnOptionPressed(object sender, EventArgs args)
    {
        option = !option;
        OnOptionStateChanged?.Invoke(this, option);
    }

    void OnAttack1(object sender, EventArgs args)
    {
        AudioManager.Instance.PlayOneShot(FModEvents.Instance.FKSwordSlash, slashSpawn.position);
        if (!option) return;
        currentHeatValue -= attack1Drain;
        OnHeatValueChanged?.Invoke(this, currentHeatValue);
    }
    void OnAttack2(object sender, EventArgs args)
    {
        if (!option) return;
        currentHeatValue -= attack2Drain;
        OnHeatValueChanged?.Invoke(this, currentHeatValue);
    }
    void OnAttack3(object sender, EventArgs args)
    {
        if (!option) return;
        currentHeatValue -= attack3Drain;
        OnHeatValueChanged?.Invoke(this, currentHeatValue);
    }

    public void SetupFKHeatbar(int index)
    {
        currentHeatValue = 0;
        heatBar.SetupHeatBar(this, index);
        OnHeatValueChanged?.Invoke(this, currentHeatValue);
    }

    public void SetFKValue(float value)
    {
        currentHeatValue = value;
        OnHeatValueChanged?.Invoke(this, currentHeatValue);
    }

    void FixedUpdate()
    {
        if (GameManager.GameState != GameState.Game) return;
        if (!option)
        {
            currentHeatValue += gainAmount * Time.deltaTime;
        }
        else
        {
            currentHeatValue -= drainAmount * Time.deltaTime;
        }
        OnHeatValueChanged?.Invoke(this, currentHeatValue);
        if (currentHeatValue > 0) return;
        currentHeatValue = 0;
        option = false;
        OnOptionStateChanged?.Invoke(this, option);
    }

    public void FireSlash()
    {
        if (!enhance) return;
        var slash = Instantiate(fireslashPrefab, slashSpawn.position, Quaternion.identity);

        DamageData data = GetDamageData(AttackType.One);
        data.Enhanced = true;
        if (option) data.Damage *= heatDamageMultiplier;

        slash.SetupFireball(data, character, IsFacingLeft,
            IsFacingLeft ? Vector2.left : Vector2.right, discSpeed, .25f, true);
        enhance = false;
    }

    public void FireDisc()
    {
        if (!enhance) return;
        var disc = Instantiate(firediscPrefab, slashSpawn.position, Quaternion.identity);
        DamageData data = GetDamageData(AttackType.Two);
        data.Enhanced = true;
        disc.SetupFireball(data, character, IsFacingLeft, IsFacingLeft ? Vector2.left : Vector2.right,
            discSpeed, .5f, true);
        enhance = false;
    }

    public void Fireball()
    {
        if(!enhance) return;
        var explosion = Instantiate(explosionPrefab, explosionSpawn.position, Quaternion.identity);
        DamageData data = GetDamageData(AttackType.Three);
        data.Enhanced = true;
        explosion.SetupFireball(data, character, IsFacingLeft, 
            Vector2.zero, 0, .5f);
        enhance = false;
    }
}