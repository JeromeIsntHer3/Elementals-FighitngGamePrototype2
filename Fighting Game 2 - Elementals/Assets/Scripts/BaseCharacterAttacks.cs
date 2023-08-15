using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAttacks : BaseCharacter
{
    [Header("Attack Prefabs")]
    [SerializeField] protected GameObject prefab;

    [Header("References")]
    [SerializeField] protected Transform rotater;
    [SerializeField] protected Transform centre;
    [SerializeField] protected CharacterAttackSO attacksData;
    [SerializeField] protected GameBoxes hitboxes;

    protected readonly Dictionary<AnimationType, AttackData> attackData = new();

    protected CharacterInput cInput;
    protected bool IsFacingLeft;

    protected delegate void Attack();
    protected Attack Attack1;
    protected Attack Attack2;
    protected Attack Attack3;
    protected Attack JumpAttack;
    protected Attack Ultimate;

    bool inAir;

    public override void Awake()
    {
        base.Awake();
        cInput = GetComponent<CharacterInput>();
        attacksData.AddToDict(attackData);
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
        cInput.OnHit += OnHit;
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
        cInput.OnHit -= OnHit;
    }

    void OnAttack1(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir)
        {
            JumpAttack?.Invoke();
            SetRecoveryDuration(GetDuration(AnimationType.JumpAttack));
            return;
        }
        Attack1?.Invoke();
        SetRecoveryDuration(GetDuration(AnimationType.Attack1));
        SetHitboxData(attackData[AnimationType.Attack1]);
    }

    void OnAttack2(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir) return;
        Attack2?.Invoke();
        SetRecoveryDuration(GetDuration(AnimationType.Attack2));
        SetHitboxData(attackData[AnimationType.Attack2]);
    }

    void OnAttack3(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir) return;
        Attack3?.Invoke();
        SetRecoveryDuration(GetDuration(AnimationType.Attack3));
        SetHitboxData(attackData[AnimationType.Attack3]);
    }

    void OnUltimate(object sender, EventArgs e)
    {
        if (!Recovered()) return;
        if (inAir) return;
        Ultimate?.Invoke();
        SetRecoveryDuration(GetDuration(AnimationType.Ultimate));
        SetHitboxData(attackData[AnimationType.Ultimate]);
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

    void OnHit(object sender, DamageData e)
    {
        SetRecoveryDuration(GetDuration(AnimationType.Damaged));
    }

    void SetHitboxData(AttackData data)
    {
        foreach(var box in hitboxes.colliders)
        {
            box.GetComponent<Hitbox>().SetAttackData(data);
        }
    }

    public void AttackImminent()
    {
        if (GameManager.Instance.DistBetweenPlayers() > GameManager.Instance.DistanceThreshold) return;

        enemy.GetComponent<CharacterInput>().OnDefend?.Invoke(this, EventArgs.Empty);
    }
}