using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<CharacterAnimation> animations;

    CharacterInput cInput;
    CharacterMovement cMovement;

    Animator animator;
    SpriteRenderer spriteRenderer;
    readonly Dictionary<AnimationType, int> animationHashes = new();
    readonly Dictionary<AnimationType, float> animationTimes = new();
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
    float recoveryTime;
    bool moving;

    Vector2 movement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cInput = GetComponent<CharacterInput>();
        cMovement = GetComponent<CharacterMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    void Start()
    {
        foreach (CharacterAnimation animation in animations)
        {
            int animHash = Animator.StringToHash(animation.Clip.name);
            animationHashes.Add(animation.Type, animHash);
            animationTimes.Add(animation.Type, animation.Clip.averageDuration);
            animationCanChangeFaceDirection.Add(animHash, animation.canChangeFaceDirection);
        }
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

        attack1 = true;
        SetRecoveryPeriod(GetDuration(AnimationType.Attack1));
    }

    void OnAttack2(object sender, EventArgs args)
    {
        if (!Recovered()) return;

        attack2 = true;
        SetRecoveryPeriod(GetDuration(AnimationType.Attack2));
    }

    void OnAttack3(object sender, EventArgs args)
    {
        if (!Recovered()) return;

        if (!grounded) return;
        attack3 = true;
        SetRecoveryPeriod(GetDuration(AnimationType.Attack3));
    }

    void OnUltimate(object sender, EventArgs args)
    {
        if (!Recovered()) return;

        if (!grounded) return;
        ultimate = true;
        SetRecoveryPeriod(GetDuration(AnimationType.Ultimate));
    }

    void OnRoll(object sender, EventArgs args)
    {
        if (!Recovered()) return;

        if (!grounded) return;
        roll = true;
        SetRecoveryPeriod(GetDuration(AnimationType.Roll));
        CancelAnimation();
    }

    void OnOption(object sender, EventArgs args)
    {
        if (Mathf.Abs(cMovement.HorizontalVelocity()) < 4) return;
        if (!Recovered()) return;

        if (!grounded) return;
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
            SetRecoveryPeriod(GetDuration(AnimationType.CharacterOptionEnd));
        }
    }

    void OnJump(object sender, EventArgs args)
    {
        if (!grounded) return;
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
            Debug.Log("Recovered");
        }
        else {
            Debug.Log("Recovering");
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
        if (cMovement.VerticalVelocity() > 1) return LockState(GetHash(AnimationType.JumpPeak), GetDuration(AnimationType.JumpPeak));
        return cMovement.VerticalVelocity() < -1 ?
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

    float GetDuration(AnimationType type)
    {
        return animationTimes[type];
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
    }

    bool CanChangeDirection(int s)
    {
        return animationCanChangeFaceDirection[s];
    }

    bool Recovered()
    {
        return Time.time > recoveryTime;
    }

    void SetRecoveryPeriod(float t)
    {
        recoveryTime = Time.time + t;
    }

    public bool IsFacingLeft()
    {
        return spriteRenderer.flipX;
    }
}