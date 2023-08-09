using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAnimator : BaseCharacter
{
    CharacterInput cInput;
    BaseCharacterMovement cMovement;

    Animator animator;
    SpriteRenderer spriteRenderer;
    readonly Dictionary<AnimationType, int> animationHashes = new();
    readonly Dictionary<int, bool> animationCanChangeFaceDirection = new();

    public bool grounded,
        attack1,
        attack2,
        attack3,
        ultimate,
        airAttack,
        roll,
        jumped,
        landed,
        option,
        optionTrigger,
        optionCancelTrigger,
        optionCancelCheck;

    int currentState;
    int previousState;
    float lockedTilTime;
    bool moving;

    Vector2 movement;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        cInput = GetComponent<CharacterInput>();
        cMovement = GetComponent<BaseCharacterMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationData.AddToDicts(new AnimationTransferData()
        {
            hashes = animationHashes,
            canFlipX = animationCanChangeFaceDirection
        });
    }

    void OnEnable()
    {
        cInput.OnMovement += OnMovement;
        cInput.OnMovementPerformed += OnMovementPerformed;
        cInput.OnMovementCanceled += OnMovementCanceled;
        cInput.OnJump += OnJump;
        cInput.OnLand += OnLand;
        cInput.OnAttack1 += OnAttack1;
        cInput.OnAttack2 += OnAttack2;
        cInput.OnAttack3 += OnAttack3;
        cInput.OnUltimate += OnUltimate;
        cInput.OnRoll += OnRoll;
        cInput.OnOption += OnOption;
        cInput.OnOptionCanceled += OnOptionCanceled;
    }

    void OnDisable()
    {
        cInput.OnMovement -= OnMovement;
        cInput.OnMovementPerformed -= OnMovementPerformed;
        cInput.OnMovementCanceled -= OnMovementCanceled;
        cInput.OnJump -= OnJump;
        cInput.OnLand -= OnLand;
        cInput.OnAttack1 -= OnAttack1;
        cInput.OnAttack2 -= OnAttack2;
        cInput.OnAttack3 -= OnAttack3;
        cInput.OnUltimate -= OnUltimate;
        cInput.OnRoll -= OnRoll;
        cInput.OnOption -= OnOption;
        cInput.OnOptionCanceled -= OnOptionCanceled;
    }

    void OnMovement(object sender, Vector2 args)
    {
        movement = args;
    }

    void OnMovementPerformed(object sender, Vector2 args)
    {
        moving = true;
    }

    void OnMovementCanceled(object sender, Vector2 args)
    {
        moving = false;
    }

    void OnAttack1(object sender, EventArgs args)
    {
        if (!Recovered()) return;
        if (!grounded)
        {
            CancelAnimation();
            airAttack = true;
            return;
        }

        CancelAnimation();
        attack1 = true;
        SetRecoveryDuration(GetDuration(AnimationType.Attack1));
    }

    void OnAttack2(object sender, EventArgs args)
    {
        if (!Recovered() || !grounded) return;
        CancelAnimation();
        attack2 = true;
        SetRecoveryDuration(GetDuration(AnimationType.Attack2));
    }

    void OnAttack3(object sender, EventArgs args)
    {
        if (!Recovered() || !grounded) return;
        CancelAnimation();
        attack3 = true;
        SetRecoveryDuration(GetDuration(AnimationType.Attack3));
    }

    void OnUltimate(object sender, EventArgs args)
    {
        if (!Recovered() || !grounded) return;
        CancelAnimation();
        ultimate = true;
        SetRecoveryDuration(GetDuration(AnimationType.Ultimate));
    }

    void OnRoll(object sender, EventArgs args)
    {
        if (!Recovered() || !grounded) return;
        roll = true;
        SetRecoveryDuration(GetDuration(AnimationType.Roll));
        CancelAnimation();
    }

    void OnOption(object sender, EventArgs args)
    {
        if (Mathf.Abs(cMovement.HorizontalVelocity()) < 4) return;
        if (!Recovered() || !grounded) return;
        optionTrigger = true;
        option = true;
    }

    void OnOptionCanceled(object sender, EventArgs args)
    {
        if (!option) return;

        if (previousState == GetHash(AnimationType.CharacterOptionStart) || 
            previousState == GetHash(AnimationType.CharacterOptionLoop))
        {
            optionCancelTrigger = true;
            optionCancelCheck = true;
            SetRecoveryDuration(GetDuration(AnimationType.CharacterOptionEnd));
        }
    }

    void OnJump(object sender, EventArgs args)
    {
        if (!grounded || !Recovered()) return;
        jumped = true;
        grounded = false;
    }

    void OnLand(object sender, EventArgs args)
    {
        CancelAnimation();
        grounded = true;
        landed = true;
    }

    void Update()
    {
        if (Recovered()) {
            Debug.Log("Animator Recovered");
        }
        else {
            Debug.Log("Animator Recovering");
        }

        if(option) recoveryTime = Time.time + .333f;

        ChangeFaceDirection(cInput.CurrentMovementInput());

        var newState = GetAnimState();

        SetAnimationConditionsFalse();

        previousState = currentState;
        if (SameState(newState)) return;
        animator.CrossFade(newState, 0, 0);
        currentState = newState;
    }

    void SetAnimationConditionsFalse()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
        roll = false;
        jumped = false;
        landed = false;
        airAttack = false;
        ultimate = false;
        optionTrigger = false;
        optionCancelTrigger = false;
    }

    int GetAnimState()
    {
        if (Time.time < lockedTilTime) return currentState;

        if(optionTrigger) return LockState(GetHash(AnimationType.CharacterOptionStart), GetDuration(AnimationType.CharacterOptionStart));
        if (option)
        {
            if (optionCancelCheck)
            {
                optionCancelCheck = false;
                option = false;
                return LockState(GetHash(AnimationType.CharacterOptionEnd), GetDuration(AnimationType.CharacterOptionEnd));
            }
            return GetHash(AnimationType.CharacterOptionLoop);
        }
        if(optionCancelTrigger) return LockState(GetHash(AnimationType.CharacterOptionEnd), GetDuration(AnimationType.CharacterOptionEnd));
        if (roll) return LockState(GetHash(AnimationType.Roll), GetDuration(AnimationType.Roll));
        if (landed) return LockState(GetHash(AnimationType.JumpEnd), GetDuration(AnimationType.JumpEnd));
        if(jumped) return LockState(GetHash(AnimationType.JumpStart), GetDuration(AnimationType.JumpStart));
        if(ultimate) return LockState(GetHash(AnimationType.Ultimate), GetDuration(AnimationType.Ultimate));
        if(attack3) return LockState(GetHash(AnimationType.Attack3), GetDuration(AnimationType.Attack3));
        if(attack2) return LockState(GetHash(AnimationType.Attack2), GetDuration(AnimationType.Attack2));
        if (attack1) return LockState(GetHash(AnimationType.Attack1), GetDuration(AnimationType.Attack1));
        if (grounded) return movement.x == 0 ? 
                GetHash(AnimationType.Idle) : GetHash(AnimationType.Run);

        //Non-Grounded Animations
        if(airAttack) return LockState(GetHash(AnimationType.JumpAttack), GetDuration(AnimationType.JumpAttack));
        if (cMovement.VerticalVelocity() > 3) return GetHash(AnimationType.JumpRising);
        if (cMovement.VerticalVelocity() > 1) return LockState(GetHash(AnimationType.JumpPeak), GetDuration(AnimationType.JumpPeak));
        return cMovement.VerticalVelocity() < 0 ?
            GetHash(AnimationType.JumpFalling)
            : LockState(GetHash(AnimationType.JumpRising), GetDuration(AnimationType.JumpRising));

        int LockState(int s, float t)
        {
            lockedTilTime = Time.time + t;
            return s;
        }
    }

    void CancelAnimation()
    {
        lockedTilTime = 0;
    }

    int GetHash(AnimationType type)
    {
        return animationHashes[type];
    }

    bool SameState(int newState)
    {
        return currentState == newState;
    }

    void ChangeFaceDirection(Vector2 v)
    {
        if (!moving || !grounded) return;
        if (currentState != 0) { if (!CanChangeDirection(currentState)) return; }
        spriteRenderer.flipX = v.x < 0;
        cInput.OnChangeFaceDirection?.Invoke(this, spriteRenderer.flipX);
    }

    bool CanChangeDirection(int s)
    {
        return animationCanChangeFaceDirection[s];
    }

    public bool IsFacingLeft()
    {
        return spriteRenderer.flipX;
    }

    #region Classes

    public class AnimationTransferData
    {
        public Dictionary<AnimationType, int> hashes = new();
        public Dictionary<AnimationType, float> durations = new();
        public Dictionary<int, bool> canFlipX = new();
    }

    #endregion
}