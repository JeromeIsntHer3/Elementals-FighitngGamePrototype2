using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAttacks : MonoBehaviour
{
    [Header("Attack Prefabs")]
    [SerializeField] protected GameObject prefab;

    [Header("References")]
    [SerializeField] protected Transform rotater;
    [SerializeField] protected Transform centre;
    [SerializeField] protected CharacterAttackSO attacksData;
    [SerializeField] protected GameBoxes hitboxes;

    [Header("Meter")]
    [SerializeField] int maxMeterValue;
    [SerializeField] int currentMeterValue;
    [SerializeField] int numberOfMeter;

    protected readonly Dictionary<AttackType, AttackData> attackData = new();

    protected BaseCharacter character;
    protected CharacterInput cInput;
    protected bool IsFacingLeft;

    protected delegate void Attack();
    protected Attack Attack1;
    protected Attack Attack2;
    protected Attack Attack3;
    protected Attack JumpAttack;
    protected Attack Ultimate;
    protected Attack Enhance1;
    protected Attack Enhance2;
    protected Attack Enhance3;
    protected Attack EnhanceJump;

    public EventHandler<OnMeterUsedArgs> OnMeterUsed;
    public class OnMeterUsedArgs: EventArgs
    {
        public int MeterCount;
        public float MeterValue;

        public OnMeterUsedArgs(int i, float f)
        {
            MeterCount = i;
            MeterValue = f;
        }
    }

    bool inAir;
    float recentAttackTime;
    float attackTime;
    AttackData currentAttack;
    Attacks currentAttackType;

    public enum Attacks
    {
        One, Two, Three, Jump
    }

    public virtual void Awake()
    {
        character = GetComponent<BaseCharacter>();
        cInput = GetComponent<CharacterInput>();
        attacksData.AddToDict(attackData);
        OnMeterUsed?.Invoke(this, new OnMeterUsedArgs(numberOfMeter ,(float) currentMeterValue/maxMeterValue));
    }

    public virtual void OnEnable()
    {
        character.OnAttack1 += OnAttack1;
        character.OnAttack2 += OnAttack2;
        character.OnAttack3 += OnAttack3;
        character.OnUltimate += OnUltimate;
        character.OnJump += OnJump;
        character.OnLand += OnLand;
        character.OnChangeFaceDirection += OnChangeFaceDir;
        character.OnEnhanceAttack += OnEnhanceAttack;
    }

    public virtual void OnDisable()
    {
        character.OnAttack1 -= OnAttack1;
        character.OnAttack2 -= OnAttack2;
        character.OnAttack3 -= OnAttack3;
        character.OnUltimate -= OnUltimate;
        character.OnJump -= OnJump;
        character.OnLand -= OnLand;
        character.OnChangeFaceDirection -= OnChangeFaceDir;
        character.OnEnhanceAttack -= OnEnhanceAttack;
    }

    void OnAttack1(object sender, EventArgs e)
    {
        if (inAir)
        {
            JumpAttack?.Invoke();
            character.SetRecoveryDuration(character.GetDuration(AnimationType.JumpAttack));
            recentAttackTime = Time.time;
            currentAttackType = Attacks.Jump;
            return;
        }
        Attack1?.Invoke();
        SetHitboxData(GetDamageData(AttackType.One));
        recentAttackTime = Time.time;
        currentAttackType = Attacks.One;
    }

    void OnAttack2(object sender, EventArgs e)
    {
        if (inAir) return;
        Attack2?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Two));
        recentAttackTime = Time.time;
        currentAttackType = Attacks.Two;
    }

    void OnAttack3(object sender, EventArgs e)
    {
        if (inAir) return;
        Attack3?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Three));
        recentAttackTime = Time.time;
        currentAttackType = Attacks.Three;
    }

    void OnUltimate(object sender, EventArgs e)
    {
        if (inAir) return;
        Ultimate?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Ultimate));
    }

    void OnJump(object sender, EventArgs e)
    {
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

    void OnEnhanceAttack(object sender, EventArgs e)
    {
        if (GameManager.EnhanceAttackThresholdDuration + recentAttackTime < Time.time) return;
        if (attackTime > Time.time) return;
        UseMeter();
    }

    void SetHitboxData(DamageData data)
    {
        foreach(var box in hitboxes.colliders)
        {
            box.GetComponent<Hitbox>().SetDamageData(data, character);
        }
    }

    public void UseMeter()
    {
        if (currentMeterValue < 50) return;
        currentMeterValue -= 50;
        if (currentMeterValue <= 0)
        {
            if (numberOfMeter > 0)
            {
                numberOfMeter--;
                currentMeterValue = 100;
            }
        }

        switch (currentAttackType)
        {
            case Attacks.One:
                attackTime = Time.time + character.GetDuration(AnimationType.Attack1);
                Enhance1?.Invoke();
                break;
            case Attacks.Two:
                attackTime = Time.time + character.GetDuration(AnimationType.Attack2);
                Enhance2?.Invoke();
                break;
            case Attacks.Three:
                attackTime = Time.time + character.GetDuration(AnimationType.Attack3);
                Enhance3?.Invoke();
                break;
            case Attacks.Jump:
                attackTime = Time.time + character.GetDuration(AnimationType.JumpAttack);
                EnhanceJump?.Invoke();
                break;
        }

        OnMeterUsed?.Invoke(this, new OnMeterUsedArgs(numberOfMeter, (float)currentMeterValue/maxMeterValue));
    }

    AttackData GetAttackData(AttackType type)
    {
        return attackData[type];
    }

    protected DamageData GetDamageData(AttackType type)
    {
        AttackData attackData = GetAttackData(type);
        return new DamageData
        {
            Damage = attackData.Damage,
            VerticalKnockback = attackData.VerticalKnockback,
            HorizontalKnockback = attackData.HorizontalKnockback,
            StunDuration = attackData.StunDuration,
            DamageType = attackData.DamageType,
            Source = character
        };
    }
}