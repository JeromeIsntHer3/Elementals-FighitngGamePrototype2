using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKAttacks : BaseCharacterAttacks
{
    [SerializeField] FKHeatBarUI heatBar;
    [SerializeField] float maxValue;
    [SerializeField] float currentHeatValue;
    [SerializeField] float drainSpeed;
    [SerializeField] float gainPerHit;
    [SerializeField][Range(1,2)] float heatMultipler;

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
    public EventHandler OnHeatBurnComplete;

    protected override void OnEnable()
    {
        base.OnEnable();

        character.OnOption += OnOptionPressed;
        character.OnAttackOne += OnAttack1;
        character.OnAttackTwo += OnAttack2;
        character.OnAttackThree += OnAttack3;

        character.OnHitEnemy += AddMeter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        character.OnOption -= OnOptionPressed;
        character.OnAttackOne -= OnAttack1;
        character.OnAttackTwo -= OnAttack2;
        character.OnAttackThree -= OnAttack3;

        character.OnHitEnemy -= AddMeter;
    }

    void Start()
    {
        heatBar.SetMeterValue(currentHeatValue / maxValue);
    }

    void OnOptionPressed(object sender, EventArgs args)
    {
        option = !option;
        OnOptionStateChanged?.Invoke(this, option);
    }

    void OnAttack1(object sender, EventArgs args)
    {
        if (!option) return;
        currentHeatValue -= attack1Drain;
        heatBar.SetMeterValue(currentHeatValue / maxValue);
    }
    void OnAttack2(object sender, EventArgs args)
    {
        if (!option) return;
        currentHeatValue -= attack2Drain;
        heatBar.SetMeterValue(currentHeatValue / maxValue);
    }
    void OnAttack3(object sender, EventArgs args)
    {
        if (!option) return;
        currentHeatValue -= attack3Drain;
        heatBar.SetMeterValue(currentHeatValue / maxValue);
    }

    public void SetupHeatbar(bool left)
    {
        currentHeatValue = 0;
        heatBar.SetupHeatBar(left);
    }

    public void AddMeter(object sender, EventArgs args)
    {
        if (option) return;
        currentHeatValue += gainPerHit;
        heatBar.SetMeterValue(currentHeatValue / maxValue);
    }

    void FixedUpdate()
    {
        if (!option) return;
        currentHeatValue -= drainSpeed * Time.deltaTime;
        heatBar.SetMeterValue(currentHeatValue / maxValue);
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
        if(option) data.Damage *= heatMultipler;

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