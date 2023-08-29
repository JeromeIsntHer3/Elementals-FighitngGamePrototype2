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

    void OnHit(object sender, DamageData e)
    {
        currentHealth -= e.Damage;
    }

    void BlockHit(object sender, DamageData e)
    {
        currentHealth -= e.Damage;
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
    }

    public void Damage(DamageData data)
    {
        character.OnHit?.Invoke(this, data);
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
    }
}