using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] int player;
    [SerializeField] CharacterAnimationSO animationData;
    protected BaseCharacter enemy;
    protected readonly Dictionary<AnimationType, float> animationDuration = new();

    [SerializeField] int hitsBlockedConsecutively;
    [SerializeField] int maxHits = 20;
    [SerializeField] float reductionPerBlockLevel;
    [SerializeField] float startingReduction;
    [SerializeField] float timeBetweenHits = 3f;
    float timeTilReset;
    float recoveryTime;
    bool isGrounded = true;
    bool isGuarding = false;
    bool isFacingLeft;
    bool isAttacking = false;
    bool broken = false;

    float damageReductionPercentage;

    #region Getters & Setters

    public bool IsGrounded { get { return isGrounded; } }
    public bool IsGuarding { get {  return isGuarding ; } }
    public bool IsFacingLeft { get { return isFacingLeft; } }
    public bool IsAttacking { get { return isAttacking ; } }
    public CharacterAnimationSO AnimationData { get { return animationData; } }
    public BaseCharacter Enemy { get { return enemy; } }
    public float DamageReduction {  get { return damageReductionPercentage; } }
    public bool DefenseBroken {  get { return broken; } }

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
    public EventHandler OnHitEnemy;
    public EventHandler OnHitBlocked;

    #endregion

    void Awake()
    {
        animationData.AddToDuration(animationDuration);
    }

    void OnEnable()
    {
        OnBlockActive += BlockActive;
        OnBlockCanceled += BlockCanceled;
        OnChangeFaceDirection += ChangeFaceDirection;
        OnBlockHit += BlockHit;
    }

    void OnDisable()
    {
        OnBlockActive -= BlockActive;
        OnBlockCanceled -= BlockCanceled;
        OnChangeFaceDirection -= ChangeFaceDirection;
        OnBlockHit -= BlockHit;
    }

    void Start()
    {
        if (player == 1)
        {
            enemy = GameManager.Instance.PlayerTwo;
        }
        else if (player == 2)
        {
            enemy = GameManager.Instance.PlayerOne;
        }
        damageReductionPercentage = startingReduction;
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
        Debug.Log($"{gameObject.name} Blocked Hit From {dd.Source.name}.");

        if(Time.time > timeTilReset)
        {
            hitsBlockedConsecutively = 0;
        }

        timeTilReset = Time.time + timeBetweenHits;
        hitsBlockedConsecutively++;
        if (hitsBlockedConsecutively >= maxHits)
        {
            hitsBlockedConsecutively = maxHits;
            broken = true;
        }
        else
        {
            broken = false;
            float reduction = startingReduction;
            reduction -= reductionPerBlockLevel * hitsBlockedConsecutively;
            damageReductionPercentage = reduction;
        }
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

    public void CancelRecovery()
    {
        recoveryTime = 0;
    }

    public void SetGroundedState(bool s)
    {
        isGrounded = s;
    }
}