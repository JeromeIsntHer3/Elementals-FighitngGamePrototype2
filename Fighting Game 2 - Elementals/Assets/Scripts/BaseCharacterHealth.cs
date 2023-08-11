using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterHealth : BaseCharacter
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    CharacterInput cInpit;
    Rigidbody2D rb;

    public override void Awake()
    {
        cInpit = GetComponent<CharacterInput>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void OnEnable()
    {
        cInpit.OnHit += OnHit;
    }

    public virtual void OnDisable()
    {
        cInpit.OnHit -= OnHit;
    }

    void OnHit(object sender, float e)
    {
        currentHealth -= e;
    }

    void Knockback(float h, float v, Vector2 dir)
    {
        rb.AddForce(dir.normalized * new Vector2(h,v), ForceMode2D.Impulse);
    }

    public void Damage(DamageData data)
    {
        Knockback(data.HorizontalKnockback, data.VerticalKnockback, data.Direction);
        cInpit.OnHit?.Invoke(this, data.Damage);
    }
}