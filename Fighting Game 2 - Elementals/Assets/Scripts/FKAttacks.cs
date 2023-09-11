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
    [SerializeField] float attackOneDrain;

    [Header("Attack 2 Enhance")]
    [SerializeField] FKFireballProjectile firediscPrefab;
    [SerializeField] float discSpeed;
    [SerializeField] float attackTwoDrain;

    [Header("Attack 3 Enhance")]
    [SerializeField] FKFireballProjectile explosionPrefab;
    [SerializeField] Transform explosionSpawn;
    [SerializeField] float attackThreeDrain;

    [Header("Ultimate")]
    [SerializeField] float ultimateHeatDrain;

    bool option;

    public EventHandler<bool> OnOptionStateChanged;
    public EventHandler<float> OnHeatValueChanged;
    public EventHandler OnHeatBurnComplete;

    public float MaxHeatValue { get { return maxHeatValue; } }


    protected override void OnEnable()
    {
        base.OnEnable();

        character.OnOption += OnOptionPressed;
        character.OnAttackPressed += OnAttack;

        Attack1 = FireSlash;
        Attack2 = FireDisc;
        Attack3 = Fireball;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        character.OnOption -= OnOptionPressed;
        character.OnAttackPressed -= OnAttack;
    }

    void OnOptionPressed(object sender, EventArgs args)
    {
        option = !option;
        OnOptionStateChanged?.Invoke(this, option);
    }

    void OnAttack(object sender, int index)
    {
        if (!option) return;

        switch (index)
        {
            case 0:
                currentHeatValue -= attackOneDrain;
                break;
            case 1:
                currentHeatValue -= attackTwoDrain;
                break;
            case 2:
                currentHeatValue -= attackThreeDrain;
                break;
            case 3:
                currentHeatValue -= ultimateHeatDrain;
                break;
        }
        
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
            if (currentHeatValue > maxHeatValue) currentHeatValue = maxHeatValue;
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

    void FireSlash()
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

    void FireDisc()
    {
        if (!enhance) return;
        var disc = Instantiate(firediscPrefab, slashSpawn.position, Quaternion.identity);
        DamageData data = GetDamageData(AttackType.Two);
        data.Enhanced = true;
        disc.SetupFireball(data, character, IsFacingLeft, IsFacingLeft ? Vector2.left : Vector2.right,
            discSpeed, .5f, true);
        enhance = false;
    }

    void Fireball()
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