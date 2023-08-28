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

    float recoveryTime;
    bool isGrounded = true;
    bool isGuarding = false;
    bool isFacingLeft;
    bool isAttacking = false;

    public bool IsGrounded { get { return isGrounded; } }
    public bool IsGuarding { get {  return isGuarding ; } }
    public bool IsFacingLeft { get { return isFacingLeft; } }
    public bool IsAttacking { get { return isAttacking ; } }

    #region Events

    public EventHandler<Vector2> OnMovement;
    public EventHandler<Vector2> OnMovementPerformed;
    public EventHandler<Vector2> OnMovementCanceled;
    public EventHandler OnAttack1;
    public EventHandler OnAttack2;
    public EventHandler OnAttack3;
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
    public EventHandler OnBlock;
    public EventHandler OnBlockCanceled;
    public EventHandler OnBlockActive;
    public EventHandler<DamageData> OnHit;
    public EventHandler<DamageData> OnBlockHit;
    public EventHandler<bool> OnChangeFaceDirection;
    public EventHandler OnHitEnemy;
    public EventHandler OnHitBlocked;

    #endregion

    public CharacterAnimationSO AnimationData { get { return animationData; } }
    public BaseCharacter Enemy { get { return enemy; } }

    void Awake()
    {
        animationData.AddToDuration(animationDuration);
        OnBlockActive += (object sender, EventArgs e) =>
        {
            isGuarding = true;
        };
        OnBlockCanceled += (object sender, EventArgs e) =>
        {
            isGuarding = false;
        };
        OnChangeFaceDirection += (object sender, bool e) =>
        {
            isFacingLeft = e;
        };
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

    public bool HasOption()
    {
        return animationData.optionIsHeld || animationData.optionIsTriggered;
    }
}