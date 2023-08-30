using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterHealth : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    public EventHandler<float> OnHealthChanged;

    BaseCharacter character;

    void Awake()
    {
        character = GetComponent<BaseCharacter>();
        currentHealth = maxHealth;
    }

    protected virtual void OnEnable()
    {
        character.OnHit += OnHit;
        character.OnBlockHit += BlockHit;
    }

    protected virtual void OnDisable()
    {
        character.OnHit -= OnHit;
        character.OnBlockHit -= BlockHit;
    }

    void OnHit(object sender, DamageData data)
    {
        currentHealth -= data.Damage;
        character.SetStunnedDuration(data.HitStunDuration);
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
    }

    void BlockHit(object sender, DamageData data)
    {
        currentHealth -= data.Damage;
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
    }
}