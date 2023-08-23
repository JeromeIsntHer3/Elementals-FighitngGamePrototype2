using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAttacks : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform rotater;
    [SerializeField] protected Transform centre;
    [SerializeField] protected CharacterAttackSO attacksData;
    [SerializeField] protected GameBoxes hitboxes;

    [Header("Meter")]
    [SerializeField] int maxMeterValue;
    [SerializeField] int currentMeterValue;
    [SerializeField] int meterCount;

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

    bool inAir;
    protected bool enhance;
    float recentAttackTime;
    float meterUsedTime;
    float attackingTilTime;
    AttackType currentAttackType;
    AnimationType currentAnimationType;

    public class OnMeterUsedArgs : EventArgs
    {
        public float amount;
        public int count;

        public OnMeterUsedArgs(float amount, int count)
        {
            this.amount = amount;
            this.count = count;
        }
    }
    public EventHandler<OnMeterUsedArgs> OnMeterUsed;


    public virtual void Awake()
    {
        character = GetComponent<BaseCharacter>();
        cInput = GetComponent<CharacterInput>();
        attacksData.AddToDict(attackData);
    }

    void Start()
    {
        OnMeterUsed?.Invoke(this, new OnMeterUsedArgs(currentMeterValue / maxMeterValue, meterCount));
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

        character.OnTryEnhance += OnTryEnhanceAttack;
        character.OnTryCancel += OnTryCancelAnimation;
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
        character.OnTryEnhance -= OnTryEnhanceAttack;
        character.OnTryCancel -= OnTryCancelAnimation;
    }

    void OnAttack1(object sender, EventArgs e)
    {
        if (inAir)
        {
            JumpAttack?.Invoke();
            SetHitboxData(GetDamageData(AttackType.Jump));
            SetNewAttack(AnimationType.JumpAttack, AttackType.Jump);
            return;
        }
        Attack1?.Invoke();
        SetHitboxData(GetDamageData(AttackType.One));
        SetNewAttack(AnimationType.Attack1, AttackType.One);
    }

    void OnAttack2(object sender, EventArgs e)
    {
        if (inAir) return;
        Attack2?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Two));
        SetNewAttack(AnimationType.Attack2, AttackType.Two);
    }

    void OnAttack3(object sender, EventArgs e)
    {
        if (inAir) return;
        Attack3?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Three));
        SetNewAttack(AnimationType.Attack3, AttackType.Three);
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

    void OnTryEnhanceAttack(object sender, EventArgs e)
    {
        if(meterCount == 0 && currentMeterValue <= 0) return;
        if (GameManager.Instance.MeterBurnThresholdTime + recentAttackTime < Time.time) return;
        if (meterUsedTime > Time.time) return;
        meterUsedTime = Time.time + character.GetAnimationDuration(currentAnimationType);
        UseMeter(GetAttackData(currentAttackType).MeterUsage, false);
        enhance = true;
    }

    void OnTryCancelAnimation(object sender, EventArgs e)
    {
        if (meterCount == 0 && currentMeterValue <= 0 || attackingTilTime < Time.time) return;
        meterUsedTime = 0;
        attackingTilTime = 0;
        UseMeter(50,true);
    }

    void SetHitboxData(DamageData data)
    {
        foreach(var box in hitboxes.colliders)
        {
            box.GetComponent<Hitbox>().SetDamageData(data, character);
        }
    }

    public void UseMeter(int usage, bool cancel)
    {
        if (meterCount < 0 || meterCount == 0 && currentMeterValue < 50) return;
        currentMeterValue -= usage;
        if (currentMeterValue <= 0)
        {
            if (meterCount > 0)
            {
                meterCount--;
                currentMeterValue = 100;
            }
        }

        OnMeterUsed?.Invoke(this,new OnMeterUsedArgs((float) currentMeterValue / maxMeterValue, meterCount));

        if (cancel)
        {
            character.OnCancelAnimation?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            character.OnEnhanceAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    void SetNewAttack(AnimationType anim, AttackType attack)
    {
        recentAttackTime = Time.time;
        attackingTilTime = Time.time + character.GetAnimationDuration(anim);
        currentAttackType = attack;
        currentAnimationType = anim;
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