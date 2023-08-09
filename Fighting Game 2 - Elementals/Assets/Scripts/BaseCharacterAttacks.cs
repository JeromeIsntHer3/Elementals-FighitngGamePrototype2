using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAttacks : BaseCharacter
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Transform rotater;
    [SerializeField] protected Transform centre;

    protected CharacterInput cInput;
    bool inAir;
    protected bool IsFacingLeft;

    protected delegate void Attack();
    protected Attack Attack1;
    protected Attack Attack2;
    protected Attack Attack3;
    protected Attack JumpAttack;
    protected Attack Ultimate;

    public override void Awake()
    {
        base.Awake();
        cInput = GetComponent<CharacterInput>();
    }

    public virtual void OnEnable()
    {
        cInput.OnAttack1 += OnAttack1;
        cInput.OnAttack2 += OnAttack2;
        cInput.OnAttack3 += OnAttack3;
        cInput.OnUltimate += OnUltimate;
        cInput.OnJump += OnJump;
        cInput.OnLand += OnLand;
        cInput.OnChangeFaceDirection += OnChangeFaceDir;
    }

    public virtual void OnDisable()
    {
        cInput.OnAttack1 -= OnAttack1;
        cInput.OnAttack2 -= OnAttack2;
        cInput.OnAttack3 -= OnAttack3;
        cInput.OnUltimate -= OnUltimate;
        cInput.OnJump -= OnJump;
        cInput.OnLand -= OnLand;
        cInput.OnChangeFaceDirection -= OnChangeFaceDir;
    }

    void OnAttack1(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir)
        {
            JumpAttack?.Invoke();
            return;
        }
        Attack1?.Invoke();
    }

    void OnAttack2(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir) return;
        Attack2?.Invoke();
    }

    void OnAttack3(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir) return;
        Attack3?.Invoke();
    }

    void OnUltimate(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir) return;
        Ultimate?.Invoke();
    }

    void OnJump(object sender, EventArgs e)
    {
        if(!Recovered()) return;
        inAir = true;
    }

    void OnLand(object sender, EventArgs e)
    {
        inAir = false;
    }

    void OnChangeFaceDir(object sender, bool e)
    {
        IsFacingLeft = e;
        rotater.localScale = IsFacingLeft ? new Vector3(-1,1,1) : new Vector3(1,1,1);
    }
}