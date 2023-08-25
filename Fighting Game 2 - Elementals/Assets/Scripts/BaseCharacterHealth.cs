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

    public virtual void OnEnable()
    {
        character.OnHit += OnHit;
    }

    public virtual void OnDisable()
    {
        character.OnHit -= OnHit;
    }

    void OnHit(object sender, DamageData e)
    {
        currentHealth -= e.Damage;
    }

    public void Damage(DamageData data)
    {
        Debug.Log(data);
        character.OnHit?.Invoke(this, data);
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
    }
}