using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBAttacks : BaseCharacterAttacks
{
    [SerializeField] MBKnife knifePrefab;
    [SerializeField] Transform knifeThrowSpawn;
    [SerializeField] Transform trapThrowSpawn;

    [Header("Values")]
    [SerializeField] float knifeSpeed;
    [SerializeField] float trapSpeed;

    int knivesThrown = 0;

    public EventHandler<bool> OnInvicibleStateChanged;

    protected override void OnEnable()
    {
        base.OnEnable();

        character.OnHit += OnHit;
        character.OnBlockHit += OnHit;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        character.OnHit -= OnHit;
        character.OnBlockHit -= OnHit;
    }

    public void ThrowKnife() 
    {
        Vector2 faceDir = IsFacingLeft ? Vector2.left : Vector2.right;
        var knife = Instantiate(knifePrefab, knifeThrowSpawn.position, Quaternion.identity);
        knife.SetupKnife(character, GetDamageData(AttackType.Two),5f, IsFacingLeft,
            faceDir.normalized,knifeSpeed, false ,enhance);

        if (enhance) enhance = false;
    }
 
    public void ThrowTrap()
    {
        Vector2 faceDir = trapThrowSpawn.position - centre.position;
        var knife = Instantiate(knifePrefab, trapThrowSpawn.position, Quaternion.identity);
        knife.SetupKnife(character, GetDamageData(AttackType.Two), 5f, IsFacingLeft,
            faceDir, trapSpeed, true);
    }

    public void SlashAndThrow()
    {
        if (!enhance) return;

        knivesThrown++;
        Vector2 faceDir = IsFacingLeft ? Vector2.left : Vector2.right;
        var knife = Instantiate(knifePrefab, knifeThrowSpawn.position, Quaternion.identity);
        knife.SetupKnife(character, GetDamageData(AttackType.OneEnhanced), 5f, IsFacingLeft,
            faceDir.normalized, knifeSpeed, false , false);

        if(knivesThrown >= 2) enhance = false;
    }

    public void Invisible()
    {
        if (!enhance) return;

        OnInvicibleStateChanged?.Invoke(this, true);

        enhance = false;
    }

    void OnHit(object sender, DamageData args)
    {
        OnInvicibleStateChanged?.Invoke(this, false);
    }
}