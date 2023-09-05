using System;
using UnityEngine;

public class BaseCharacterHealth : MonoBehaviour
{
    protected float currentHealth;
    float maxHealth;

    public EventHandler<float> OnHealthChanged;

    BaseCharacter character;
    public float CurrentHealth { get { return currentHealth; } }


    public void SetupHealth(float curr, float max)
    {
        currentHealth = curr;
        maxHealth = max;
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
    }

    void Awake()
    {
        character = GetComponent<BaseCharacter>();
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
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            character.OnDeath?.Invoke(this, EventArgs.Empty);
            GameUI.OnPlayerDeath?.Invoke(this, GetComponent<CharacterInput>().PlayerIndex);
        }
        character.SetStunnedDuration(data.HitStunDuration);
    }

    void BlockHit(object sender, DamageData data)
    {
        currentHealth -= data.Damage;
        OnHealthChanged?.Invoke(this, currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            character.OnDeath?.Invoke(this, EventArgs.Empty);
            GameUI.OnPlayerDeath?.Invoke(this, GetComponent<CharacterInput>().PlayerIndex);
        }
    }
}