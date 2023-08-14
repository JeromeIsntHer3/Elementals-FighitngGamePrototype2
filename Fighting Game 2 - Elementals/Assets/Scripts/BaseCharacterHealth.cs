using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterHealth : BaseCharacter
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    CharacterInput cInput;

    public EventHandler<float> OnDamaged;

    public override void Awake()
    {
        cInput = GetComponent<CharacterInput>();
        currentHealth = maxHealth;
    }

    public virtual void OnEnable()
    {
        cInput.OnHit += OnHit;
    }

    public virtual void OnDisable()
    {
        cInput.OnHit -= OnHit;
    }

    void OnHit(object sender, DamageData e)
    {
        currentHealth -= e.Damage;
    }

    public void Damage(DamageData data)
    {
        cInput.OnHit?.Invoke(this, data);
        OnDamaged?.Invoke(this, currentHealth / maxHealth);
    }
}