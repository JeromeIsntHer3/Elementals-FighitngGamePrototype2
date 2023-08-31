using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] int hitsBlockedConsecutively = 0;
    [SerializeField] CharacterAnimationSO animationData;
    protected BaseCharacter enemy;
    [SerializeField] Transform characterCentre;
    protected readonly Dictionary<AnimationType, float> animationDuration = new();


    int comboHit = 0;
    float recoveryTime;
    float stunTilTime;
    float currentDamageReductionPercentage;
    float timeLastBlockedHit;
    bool isGrounded = true;
    bool isGuarding = false;
    bool isFacingLeft;
    bool broken = false;
    bool isAttacking = false;

    BaseCharacterAttacks m_Attacks;
    BaseCharacterAttacks enemyAttacks;

    #region Getters & Setters

    public bool IsGrounded { get { return isGrounded; } }
    public bool IsGuarding { get {  return isGuarding ; } }
    public bool IsFacingLeft { get { return isFacingLeft; } }
    public CharacterAnimationSO AnimationData { get { return animationData; } }
    public BaseCharacter Enemy { get { return enemy; } }
    public float DamageReduction {  get { return currentDamageReductionPercentage; } }
    public bool DefenseBroken {  get { return broken; } }
    public int ComboHit { get {  return comboHit; } }
    public bool IsAttacking {  get { return isAttacking; } }

    public Transform Centre { get { return characterCentre; } }

    #endregion

    #region Events

    public EventHandler<Vector2> OnMovement;
    public EventHandler<Vector2> OnMovementPerformed;
    public EventHandler<Vector2> OnMovementCanceled;
    public EventHandler OnAttackOne;
    public EventHandler OnAttackTwo;
    public EventHandler OnAttackThree;
    public EventHandler OnUltimate;
    public EventHandler OnTryEnhance;
    public EventHandler OnEnhanceAttack;
    public EventHandler OnTryCancel;
    public EventHandler OnCancelAnimation;
    public EventHandler OnRoll;
    public EventHandler OnOption;
    public EventHandler OnOptionCanceled;
    public EventHandler OnJump;
    public EventHandler OnLand;
    public EventHandler OnBlockPerformed;
    public EventHandler OnBlockActive;
    public EventHandler OnBlockCanceled;
    public EventHandler<DamageData> OnHit;
    public EventHandler<DamageData> OnBlockHit;
    public EventHandler<bool> OnChangeFaceDirection;
    public EventHandler<BaseCharacter> OnHitEnemy;
    public EventHandler<BaseCharacter> OnHitBlocked;
    public EventHandler<int> OnHitCombo;
    public EventHandler<string> OnHitType;

    #endregion

    void Awake()
    {
        animationData.AddToDuration(animationDuration);
    }

    public void SetupCharacter(BaseCharacter enemy)
    {
        this.enemy = enemy;
        enemyAttacks = enemy.GetComponent<BaseCharacterAttacks>();
    }

    void OnEnable()
    {
        OnBlockActive += BlockActive;
        OnBlockCanceled += BlockCanceled;
        OnChangeFaceDirection += ChangeFaceDirection;
        OnBlockHit += BlockHit;
        OnHitEnemy += HitEnemy;
        OnHit += Hit;
    }

    void OnDisable()
    {
        OnBlockActive -= BlockActive;
        OnBlockCanceled -= BlockCanceled;
        OnChangeFaceDirection -= ChangeFaceDirection;
        OnBlockHit -= BlockHit;
        OnHitEnemy -= HitEnemy;
        OnHit -= Hit;
    }

    void Start()
    {
        currentDamageReductionPercentage = GameManager.BaseDamageReduction;
        InvokeRepeating(nameof(CheckCombo), 1f, .1f);
    }

    void BlockActive(object sender, EventArgs e)
    {
        isGuarding = true;
    }

    void BlockCanceled(object sender, EventArgs e)
    {
        isGuarding = false;
    }

    void ChangeFaceDirection(object sender, bool e)
    {
        isFacingLeft = e;
    }

    void BlockHit(object sender, DamageData dd)
    {
        Invoke(nameof(CheckBlock), GameManager.BlockResetDuration);

        hitsBlockedConsecutively++;
        timeLastBlockedHit = Time.time;

        if (hitsBlockedConsecutively >= GameManager.MaxBlockHits)
        {
            hitsBlockedConsecutively = GameManager.MaxBlockHits;
            broken = true;
        }
        else
        {
            broken = false;
            float reduction = GameManager.BaseDamageReduction;
            reduction -= GameManager.BaseDamageReductionPerLevel * hitsBlockedConsecutively;
            currentDamageReductionPercentage = reduction;
        }

        Debug.Log(currentDamageReductionPercentage);
    }

    void HitEnemy(object sender, BaseCharacter enemy)
    {
        if (enemy.Stunned())
        {
            comboHit++;
            OnHitCombo?.Invoke(this, comboHit);
        }
        else
        {
            comboHit = 1;
        }
        if (!enemy.IsAttacking) return;
        OnHitType?.Invoke(this, "COUNTER");
        Invoke(nameof(CheckHitStateType), 1f);
    }

    void CheckBlock()
    {
        if (timeLastBlockedHit + GameManager.BlockResetDuration > Time.time) return;
        hitsBlockedConsecutively = 0;
    }

    void CheckCombo()
    {
        if (!enemy) return;
        if (enemy.Stunned()) return;
        comboHit = 0;
        OnHitCombo?.Invoke(this, comboHit);
    }

    void CheckHitStateType()
    {
        OnHitType?.Invoke(this, "");
    }

    void Hit(object sender, DamageData e)
    {
        isAttacking = false;
    }

    public float GetAnimationDuration(AnimationType t)
    {
        return animationDuration[t];
    }

    public void SetRecoveryDuration(float t)
    {
        recoveryTime = Time.time + t;
    }

    public bool Recovered()
    {
        return Time.time > recoveryTime;
    }

    public void SetStunnedDuration(float t)
    {
        stunTilTime = Time.time + t;
    }

    public bool Stunned()
    {
        return Time.time < stunTilTime;
    }

    public void CancelRecovery()
    {
        recoveryTime = 0;
    }

    public void SetGroundedState(bool s)
    {
        isGrounded = s;
    }

    public void IncreaseCombo()
    {
        comboHit++;
    }

    public void SetAttackingState(int state)
    {
        isAttacking = state switch
        {
            0 => false,
            1 => true,
            _ => false,
        };
    }
}