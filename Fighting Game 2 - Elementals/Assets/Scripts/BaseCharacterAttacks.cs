using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAttacks : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform rotater;
    [SerializeField] protected Transform centre;
    [SerializeField] protected CharacterAttackSO attacksData;
    [SerializeField] protected GameBoxes hitboxes;

    int currentMeterValue;
    int currentMeterCount;

    protected readonly Dictionary<AttackType, AttackData> attackData = new();

    protected BaseCharacter character;
    protected CharacterInput cInput;
    protected AttackType currentAttackType;
    protected bool IsFacingLeft;
    protected bool enhance;

    protected delegate void Attack();
    protected Attack Attack1;
    protected Attack Attack2;
    protected Attack Attack3;
    protected Attack JumpAttack;
    protected Attack Ultimate;

    bool inAir;
    float recentAttackTime;
    float meterUsedTime;
    float attackingTilTime;
    
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
    public EventHandler<OnMeterUsedArgs> OnMeterValueChanged;


    protected virtual void Awake()
    {
        character = GetComponent<BaseCharacter>();
        cInput = GetComponent<CharacterInput>();
        attacksData.AddToDict(attackData);
    }

    public void SetupMeter(int currValue, int currCount)
    {
        currentMeterValue = currValue;
        currentMeterCount = currCount;
        OnMeterValueChanged?.Invoke(this,
            new OnMeterUsedArgs(currentMeterValue, currentMeterCount));
    }

    protected virtual void OnEnable()
    {
        character.OnAttackOne += OnAttack1;
        character.OnAttackTwo += OnAttack2;
        character.OnAttackThree += OnAttack3;
        character.OnUltimate += OnUltimate;

        character.OnJump += OnJump;
        character.OnLand += OnLand;

        character.OnChangeFaceDirection += OnChangeFaceDir;

        character.OnTryEnhance += OnTryEnhanceAttack;
        character.OnTryCancel += OnTryCancelAnimation;

        character.OnHitEnemy += OnHitEnemy;
        character.OnHitBlocked += OnHitBlocked;
        character.OnHit += OnHit;
        character.OnBlockHit += OnEnemyBlockHit;
    }

    protected virtual void OnDisable()
    {
        character.OnAttackOne -= OnAttack1;
        character.OnAttackTwo -= OnAttack2;
        character.OnAttackThree -= OnAttack3;
        character.OnUltimate -= OnUltimate;
        character.OnJump -= OnJump;
        character.OnLand -= OnLand;
        character.OnChangeFaceDirection -= OnChangeFaceDir;
        character.OnTryEnhance -= OnTryEnhanceAttack;
        character.OnTryCancel -= OnTryCancelAnimation;
        character.OnHitEnemy -= OnHitEnemy;
        character.OnHitBlocked -= OnHitBlocked;
        character.OnHit -= OnHit;
        character.OnBlockHit -= OnEnemyBlockHit;
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
        if (inAir)
        {
            JumpAttack?.Invoke();
            SetHitboxData(GetDamageData(AttackType.Jump));
            SetNewAttack(AnimationType.JumpAttack, AttackType.Jump);
            return;
        }
        Attack2?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Two));
        SetNewAttack(AnimationType.Attack2, AttackType.Two);
    }

    void OnAttack3(object sender, EventArgs e)
    {
        if (inAir)
        {
            JumpAttack?.Invoke();
            SetHitboxData(GetDamageData(AttackType.Jump));
            SetNewAttack(AnimationType.JumpAttack, AttackType.Jump);
            return;
        }
        Attack3?.Invoke();
        SetHitboxData(GetDamageData(AttackType.Three));
        SetNewAttack(AnimationType.Attack3, AttackType.Three);
    }

    void OnUltimate(object sender, EventArgs e)
    {
        if (inAir) return;
        if (currentMeterCount == 0 && currentMeterValue < 100) return;
        Ultimate?.Invoke();
        UseMeter(100, false);
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
        if(currentMeterCount == 0 && currentMeterValue < 100) return;
        if (GameManager.MeterBurnThresholdTime + recentAttackTime < Time.time) return;
        if (meterUsedTime > Time.time) return;
        meterUsedTime = Time.time + character.GetAnimationDuration(currentAnimationType);
        UseMeter(100, false);
        enhance = true;
    }

    void OnTryCancelAnimation(object sender, EventArgs e)
    {
        if (currentMeterCount == 0 && currentMeterValue <= 0 || attackingTilTime < Time.time) return;
        meterUsedTime = 0;
        attackingTilTime = 0;
        UseMeter(50,true);
    }

    void OnHit(object sender, DamageData e)
    {
        GainMeter(GameManager.BaseMeterGainOnHit);
        OnMeterValueChanged?.Invoke(this, new OnMeterUsedArgs(
            currentMeterValue, currentMeterCount));
    }

    void OnHitBlocked(object sender, BaseCharacter e)
    {
        GainMeter(GameManager.BaseMeterGainOnBlockHit);
        OnMeterValueChanged?.Invoke(this, new OnMeterUsedArgs(
            currentMeterValue, currentMeterCount));
    }

    void OnHitEnemy(object sender, BaseCharacter e)
    {
        GainMeter(GameManager.BaseMeterGainOnEnemyHit);
        OnMeterValueChanged?.Invoke(this, new OnMeterUsedArgs(
            currentMeterValue, currentMeterCount));
    }

    void OnEnemyBlockHit(object sender, DamageData e)
    {
        GainMeter(GameManager.BaseMeterGainOnEnemyBlockHit);
        OnMeterValueChanged?.Invoke(this, new OnMeterUsedArgs(
            currentMeterValue, currentMeterCount));
    }

    void SetHitboxData(DamageData data)
    {
        foreach(var box in hitboxes.colliders)
        {
            box.GetComponent<Hitbox>().SetDamageData(data, character);
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
            HitStunDuration = attackData.HitStunDuration,
            BlockStunDuration = attackData.BlockStunDuration,
            Source = character
        };
    }

    void GainMeter(int amount)
    {
        currentMeterValue += amount;

        if (currentMeterValue > GameManager.MaxMeterValue)
        {
            if (currentMeterCount >= GameManager.MaxMeterCount)
            {
                currentMeterValue = 100;
            }
            else
            {
                int meterProfit = currentMeterValue - GameManager.MaxMeterValue;
                currentMeterCount++;
                currentMeterValue = meterProfit;
            }
        }
    }

    public void UseMeter(int usage, bool cancel)
    {
        if (currentMeterCount < 0) return;
        if (currentMeterCount <= 0 && currentMeterValue < 50) return;

        if (usage > currentMeterValue && currentMeterCount == 0) return;

        currentMeterValue -= usage;

        if(currentMeterCount > 0)
        {
            if(currentMeterValue < 0)
            {
                int meterDeficit = currentMeterValue;
                currentMeterCount--;
                currentMeterValue = 100 + meterDeficit;
            }
        }

        if (cancel)
        {
            character.OnCancelAnimation?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            character.OnEnhanceAttack?.Invoke(this, EventArgs.Empty);
        }

        OnMeterValueChanged?.Invoke(this, new OnMeterUsedArgs(
            (float) currentMeterValue, currentMeterCount));
    }
}