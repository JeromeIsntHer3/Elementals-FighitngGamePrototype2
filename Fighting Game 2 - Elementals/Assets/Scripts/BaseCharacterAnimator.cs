using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterAnimator : MonoBehaviour
{
    protected CharacterInput cInput;
    protected BaseCharacterMovement cMovement;
    protected BaseCharacter character;
    protected Rigidbody2D rb;

    readonly Dictionary<AnimationType, int> animationHashes = new();
    readonly Dictionary<AnimationType, bool> animationCanChangeFaceDirection = new();
    readonly Dictionary<AnimationType, bool> animationFullyAnimate = new();
    readonly Dictionary<AnimationType, bool> animCond = new();

    Animator animator;
    SpriteRenderer spriteRenderer;
    AnimationType currentState;
    AnimationType previousState;
    bool attacking = false;
    bool grounded = true;
    float lockedTilTime;
    float stunTilTime;
    float blockTilTime;
    float attackingTilTime;
    Vector2 movement;

    public delegate bool OptionCondition();
    protected OptionCondition OptionPerformCondition;
    protected OptionCondition OptionCancelCondition;


    public virtual void Awake()
    {
        character = GetComponent<BaseCharacter>();
        animator = GetComponent<Animator>();
        cInput = GetComponent<CharacterInput>();
        cMovement = GetComponent<BaseCharacterMovement>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        //Add Animations Here
        character.AnimationData.AddToHashesDict(animationHashes);
        character.AnimationData.AddToCanChangeDirectionDict(animationCanChangeFaceDirection);
        character.AnimationData.AddToFullyAnimate(animationFullyAnimate);
        character.AnimationData.AddToConditionList(animCond);
    }

    public virtual void OnEnable()
    {
        character.OnMovement += OnMovement;
        character.OnJump += OnJump;
        character.OnLand += OnLand;

        character.OnAttack1 += OnAttack1;
        character.OnAttack2 += OnAttack2;
        character.OnAttack3 += OnAttack3;
        character.OnUltimate += OnUltimate;

        character.OnRoll += OnRoll;

        character.OnHit += OnHit;
        character.OnBlock += OnBlock;
        character.OnBlockHit += OnBlockHit;
        character.OnBlockCanceled += OnBlockCanceled;

        character.OnEnhanceAttack += OnEnhanceAttack;
        character.OnCancelAnimation += OnAnimationCancel;

        if (character.AnimationData.optionIsHeld)
        {
            character.OnOption += OnOption;
            character.OnOptionCanceled += OnOptionCanceled;
        }
        else if(character.AnimationData.optionIsTriggered)
        {
            character.OnOption += OnOption;
        }
    }

    public virtual void OnDisable()
    {
        character.OnMovement -= OnMovement;
        character.OnJump -= OnJump;
        character.OnLand -= OnLand;
        character.OnAttack1 -= OnAttack1;
        character.OnAttack2 -= OnAttack2;
        character.OnAttack3 -= OnAttack3;
        character.OnUltimate -= OnUltimate;
        character.OnRoll -= OnRoll;
        character.OnHit -= OnHit;
        character.OnBlock -= OnBlock;
        character.OnBlockHit -= OnBlockHit;
        character.OnBlockCanceled -= OnBlockCanceled;
        character.OnEnhanceAttack -= OnEnhanceAttack;
        character.OnCancelAnimation -= OnAnimationCancel;

        if (character.AnimationData.optionIsHeld)
        {
            character.OnOption -= OnOption;
            character.OnOptionCanceled -= OnOptionCanceled;
        }
        else if (character.AnimationData.optionIsTriggered)
        {
            character.OnOption -= OnOption;
        }
    }

    void OnMovement(object sender, Vector2 args)
    {
        movement = args;
    }

    void OnAttack1(object sender, EventArgs args)
    {
        if (!grounded)
        {
            
            CancelAnimation();
            animCond[AnimationType.JumpAttack] = true;
            attacking = true;
            attackingTilTime = Time.time + character.GetAnimationDuration(AnimationType.JumpAttack);
            return;
        }

        CancelAnimation();
        animCond[AnimationType.Attack1] = true;
        attacking = true;
        attackingTilTime = Time.time + character.GetAnimationDuration(AnimationType.Attack1);
    }

    void OnAttack2(object sender, EventArgs args)
    {
        if (!grounded) return;
        CancelAnimation();
        animCond[AnimationType.Attack2] = true;
        attacking = true;
        attackingTilTime = Time.time + character.GetAnimationDuration(AnimationType.Attack1);
    }

    void OnAttack3(object sender, EventArgs args)
    {
        if (!grounded) return;
        CancelAnimation();
        animCond[AnimationType.Attack3] = true;
        attacking = true;
        attackingTilTime = Time.time + character.GetAnimationDuration(AnimationType.Attack1);
    }

    void OnUltimate(object sender, EventArgs args)
    {
        if (!grounded) return;
        CancelAnimation();
        animCond[AnimationType.Ultimate] = true;
    }

    void OnRoll(object sender, EventArgs args)
    {
        if (!grounded) return;
        animCond[AnimationType.Roll] = true;
        CancelAnimation();
    }

    void OnOption(object sender, EventArgs args)
    {
        if (!OptionPerformCondition() && !grounded) return;
        animCond[AnimationType.CharacterOptionStart] = true;
        if (character.AnimationData.optionIsTriggered)
        {
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.CharacterOptionStart));
            return;
        }
        animCond[AnimationType.CharacterOptionLoop] = true;
    }

    void OnOptionCanceled(object sender, EventArgs args)
    {
        if (!animCond[AnimationType.CharacterOptionLoop]) return;

        animCond[AnimationType.CharacterOptionEnd] = true;
        animCond[AnimationType.CharacterOptionLoop] = false;
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.CharacterOptionEnd));
    }

    void OnJump(object sender, EventArgs args)
    {
        if (!grounded || !character.Recovered()) return;
        animCond[AnimationType.JumpStart] = true;
        grounded = false;
    }

    void OnLand(object sender, EventArgs args)
    {
        CancelAnimation();
        grounded = true;
        character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.JumpEnd));
        animCond[AnimationType.JumpEnd] = true;
    }

    void OnHit(object sender, DamageData args)
    {
        CancelAnimation();
        animCond[AnimationType.Hit] = true;
        animCond[AnimationType.RecoveryFromHit] = true;
        stunTilTime = Time.time + args.StunDuration;
        character.SetRecoveryDuration(args.StunDuration);
        ShakeOnHit(args.StunDuration);
        DamageFlash();
    }

    void OnBlock(object sender, EventArgs e)
    {
        rb.velocity = Vector2.zero;
        //blockTrigger = true;
        animCond[AnimationType.DefendStart] = true;
        animCond[AnimationType.DefendLoop] = true;
    }

    void OnBlockCanceled(object sender, EventArgs e)
    {
        if (!animCond[AnimationType.DefendLoop]) return;
        animCond[AnimationType.DefendEnd] = true;
        animCond[AnimationType.DefendLoop] = false;
    }

    void OnBlockHit(object sender, DamageData data)
    {
        CancelAnimation();
        animCond[AnimationType.DefendHit] = true;
        blockTilTime = Time.time + data.StunDuration * GameManager.Instance.HitShakeAnim / 100;
        ShakeOnHit(data.StunDuration * GameManager.Instance.HitShakeAnim / 100);
        character.SetRecoveryDuration(data.StunDuration);
    }

    void ShakeOnHit(float duration)
    {
        spriteRenderer.transform.DOKill();
        spriteRenderer.transform.DOShakePosition(Mathf.Clamp(duration * GameManager.Instance.HitShakeAnim / 100,
            .01f, 2f), 
            new Vector3(GameManager.Instance.ShakeStrengthX,
            GameManager.Instance.ShakeStrengthY, 0),
            GameManager.Instance.Vibrato, default, false, true, ShakeRandomnessMode.Harmonic)
            .OnComplete(() =>
            {
                spriteRenderer.transform.localPosition = Vector3.zero;
            });
    }

    void DamageFlash()
    {
        SetSpriteColour(Color.red, .3f);
    }

    void OnEnhanceAttack(object sender, EventArgs args)
    {
        SetSpriteColour(Color.yellow, .5f);
    }

    void OnAnimationCancel(object sender, EventArgs e)
    {
        if (!attacking) return;
        CancelAnimation();
        character.CancelRecovery();
        attacking = false;
        SetSpriteColour(Color.cyan, .2f);
    }

    void SetSpriteColour(Color color, float totalDuration)
    {
        Color prevColor = spriteRenderer.color;
        spriteRenderer.DOKill();
        spriteRenderer.DOColor(color, totalDuration/2).OnComplete(() =>
        {
            spriteRenderer.DOColor(prevColor, totalDuration / 2);
        });
    }

    void Update()
    {
        if (cInput.CurrentMovementInput().x == 0)
        {
            if (character.Enemy != null)
            {
                Vector2 enemyDir = (character.Enemy.transform.position - transform.position).normalized;
                ChangeFaceDirection(new Vector2(enemyDir.x, 0));
            }
        }
        else
        {
            ChangeFaceDirection(cInput.CurrentMovementInput());
        }

        if (attackingTilTime < Time.time) attacking = false;
        if (stunTilTime < Time.time) animCond[AnimationType.Hit] = false;
        if (blockTilTime < Time.time) animCond[AnimationType.DefendHit] = false;

        if (animCond[AnimationType.DefendLoop]) character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.DefendEnd));
        if (character.AnimationData.optionIsHeld && animCond[AnimationType.CharacterOptionLoop])
        {
            character.SetRecoveryDuration(character.GetAnimationDuration(AnimationType.CharacterOptionEnd));
        }

        var newState = GetAnimation();

        SetAnimationConditionsFalse();

        previousState = currentState;
        if (SameState(newState)) return;
        Debug.Log(newState);
        animator.CrossFade(GetHashAndSetLockTime(newState), 0, 0);
        currentState = newState;
    }

    void SetAnimationConditionsFalse()
    {
        animCond[AnimationType.Attack1] = false;
        animCond[AnimationType.Attack2] = false;
        animCond[AnimationType.Attack3] = false;
        animCond[AnimationType.Roll] = false;
        animCond[AnimationType.JumpStart] = false;
        animCond[AnimationType.JumpEnd] = false;
        animCond[AnimationType.JumpAttack] = false;
        animCond[AnimationType.Ultimate] = false;
        animCond[AnimationType.CharacterOptionStart] = false;
        animCond[AnimationType.CharacterOptionEnd] = false;
        animCond[AnimationType.DefendStart] = false;
        animCond[AnimationType.DefendEnd] = false;
    }

    int GetHashAndSetLockTime(AnimationType t)
    {
        if (animationFullyAnimate[t]) lockedTilTime = Time.time + character.GetAnimationDuration(t);
        return animationHashes[t];
    }

    AnimationType GetAnimation()
    {
        if (Time.time < lockedTilTime) return currentState;

        if (animCond[AnimationType.Hit]) return AnimationType.Hit;
        if (animCond[AnimationType.RecoveryFromHit])
        {
            animCond[AnimationType.RecoveryFromHit] = false;
            return AnimationType.RecoveryFromHit;
        }

        if (animCond[AnimationType.DefendHit]) return AnimationType.DefendHit;
        if (animCond[AnimationType.DefendEnd]) return AnimationType.DefendEnd;
        if (animCond[AnimationType.DefendStart]) return AnimationType.DefendStart;
        if (animCond[AnimationType.DefendLoop]) return AnimationType.DefendLoop;

        if (character.AnimationData.optionIsHeld)
        {
            if (animCond[AnimationType.CharacterOptionEnd]) return AnimationType.CharacterOptionEnd;
            if (animCond[AnimationType.CharacterOptionStart]) return AnimationType.CharacterOptionStart;
            if (animCond[AnimationType.CharacterOptionLoop]) return AnimationType.CharacterOptionLoop;
        }

        if (character.AnimationData.optionIsTriggered)
        {
            if (animCond[AnimationType.CharacterOptionStart]) return AnimationType.CharacterOptionStart;
        }

        if (animCond[AnimationType.Roll]) return AnimationType.Roll;
        if (animCond[AnimationType.JumpEnd]) return AnimationType.JumpEnd;
        if (animCond[AnimationType.JumpStart]) return AnimationType.JumpStart;

        if (animCond[AnimationType.Ultimate]) return AnimationType.Ultimate;
        if (animCond[AnimationType.Attack1]) return AnimationType.Attack1;
        if (animCond[AnimationType.Attack2]) return AnimationType.Attack2;
        if (animCond[AnimationType.Attack3]) return AnimationType.Attack3;

#if false
        if (death) return GetHash(AnimationType.Death);
        if (hit) return LockState(GetHash(AnimationType.Hit), character.GetAnimationDuration(AnimationType.Hit));
        if (hitStun) return LockState(GetHash(AnimationType.RecoveryFromHit), character.GetAnimationDuration(AnimationType.RecoveryFromHit));

        if (blockStun) return GetHash(AnimationType.BlockOnHit);
        if (blockCancel) return LockState(GetHash(AnimationType.DefendEnd), character.GetAnimationDuration(AnimationType.DefendEnd));
        if (blocking)
        {
            if (!blockStarted)
            {
                blockStarted = true;
                character.OnBlockActive?.Invoke(this, EventArgs.Empty);
                return LockState(GetHash(AnimationType.DefendStart), character.GetAnimationDuration(AnimationType.DefendStart));
            }
            return GetHash(AnimationType.DefendLoop);
        }

        //if (blockTrigger) return LockState(GetHash(AnimationType.DefendStart), character.GetAnimationDuration(AnimationType.DefendStart));
        //if (blocking) return GetHash(AnimationType.DefendLoop);

        if (optionTrigger) return LockState(GetHash(AnimationType.CharacterOptionStart), character.GetAnimationDuration(AnimationType.CharacterOptionStart));
        if (option)
        {
            if (optionCancelCheck)
            {
                optionCancelCheck = false;
                option = false;
                return LockState(GetHash(AnimationType.CharacterOptionEnd), character.GetAnimationDuration(AnimationType.CharacterOptionEnd));
            }
            return GetHash(AnimationType.CharacterOptionLoop);
        }
        if(optionCancelTrigger) return LockState(GetHash(AnimationType.CharacterOptionEnd), character.GetAnimationDuration(AnimationType.CharacterOptionEnd));

        if (roll) return LockState(GetHash(AnimationType.Roll), character.GetAnimationDuration(AnimationType.Roll));
        if (landed) return LockState(GetHash(AnimationType.JumpEnd), character.GetAnimationDuration(AnimationType.JumpEnd));
        if(jumped) return LockState(GetHash(AnimationType.JumpStart), character.GetAnimationDuration(AnimationType.JumpStart));
        if(ultimate) return LockState(GetHash(AnimationType.Ultimate), character.GetAnimationDuration(AnimationType.Ultimate));
        if(attack3) return LockState(GetHash(AnimationType.Attack3), character.GetAnimationDuration(AnimationType.Attack3));
        if(attack2) return LockState(GetHash(AnimationType.Attack2), character.GetAnimationDuration(AnimationType.Attack2));
        if (attack1) return LockState(GetHash(AnimationType.Attack1), character.GetAnimationDuration(AnimationType.Attack1));
        if (grounded) return movement.x == 0 ? 
                GetHash(AnimationType.Idle) : GetHash(AnimationType.Run);

        //Non-Grounded Animations
        if(airAttack) return LockState(GetHash(AnimationType.JumpAttack), character.GetAnimationDuration(AnimationType.JumpAttack));

#endif
        if (grounded) return movement.x == 0 ? AnimationType.Idle : AnimationType.Run;
        if (animCond[AnimationType.JumpAttack]) return AnimationType.JumpAttack;
        if (rb.velocity.y > 3f) return AnimationType.JumpRising;
        if (rb.velocity.y > 1f) return AnimationType.JumpPeak;
        return rb.velocity.y < -1f ? AnimationType.JumpFalling : AnimationType.JumpRising;
    }

    void CancelAnimation()
    {
        lockedTilTime = 0;
    }

    bool SameState(AnimationType newState)
    {
        return currentState == newState;
    }

    void ChangeFaceDirection(Vector2 v)
    {
        if (v == Vector2.zero || !grounded) return;
        if (currentState != 0) { if (!CanChangeDirection(currentState)) return; }
        spriteRenderer.flipX = v.x <= 0;
        character.OnChangeFaceDirection?.Invoke(this, spriteRenderer.flipX);
    }

    bool CanChangeDirection(AnimationType t)
    {
        return animationCanChangeFaceDirection[t];
    }
}