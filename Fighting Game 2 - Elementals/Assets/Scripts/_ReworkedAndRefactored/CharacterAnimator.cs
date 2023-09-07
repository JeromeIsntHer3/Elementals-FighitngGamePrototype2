using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] CharacterAnimationSO animationData;

    readonly Dictionary<AnimationType, bool> animationCanChangeFaceDirection = new();
    readonly Dictionary<AnimationType, CharacterAnimation> animations = new();

    SpriteRenderer spriteRenderer;
    Animator animator;
    CharacterAnimation currentAnimation, newAnimation;
    float lockedTilTime;

    public SpriteRenderer Sr { get { return spriteRenderer; } }

    void Awake()
    {
        //Add Animations Here
        animationData.InitializeHashes();
        animationData.AddToCanChangeDirectionDict(animationCanChangeFaceDirection);

        foreach(var animation in animationData.CharacterAnimations)
        {
            animations.Add(animation.Type, animation);
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    //SWAP BACK TO USING CONDITIONS PLSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
    void Update()
    {
        if (Time.time < lockedTilTime) return;
        if (currentAnimation == newAnimation) return;
        if (newAnimation.IsFullyAnimated)
        {
            lockedTilTime = Time.time + newAnimation.Clip.averageDuration;
            Debug.Log(newAnimation.Type);
        }
        animator.CrossFade(newAnimation.AnimationHash, 0, 0);
        currentAnimation = newAnimation;
    }

    //void ShakeOnHit(float duration)
    //{
    //    spriteRenderer.transform.DOKill();
    //    spriteRenderer.transform.DOShakePosition(Mathf.Clamp(duration * GameManager.OnHitShakeAnimationMultiplier,
    //        .01f, 2f), new Vector3(GameManager.OnHitShakeStrengthX,
    //        GameManager.OnHitShakeStrengthY, 0),
    //        GameManager.OnHitVibrato, default, false, true, ShakeRandomnessMode.Harmonic)
    //        .OnComplete(() =>
    //        {
    //            spriteRenderer.transform.localPosition = Vector3.zero;
    //        });
    //}

    //void DamageFlash()
    //{
    //    SetSpriteColour(Color.red, .3f);
    //}

    //void SetSpriteColour(Color color, float totalDuration)
    //{
    //    spriteRenderer.DOKill(true);
    //    spriteRenderer.DOColor(color, totalDuration / 2).OnComplete(() =>
    //    {
    //        spriteRenderer.DOColor(Color.white, totalDuration / 2);
    //    });
    //}

    public void SetAnimation(AnimationType type)
    {
        newAnimation = animations[type];
    }

    //AnimationType GetAnimation()
    //{
    //    //if (Time.time < lockedTilTime) return currentAnimation;

    //    if (animCond[AnimationType.Death]) return AnimationType.Death;

    //    if (animCond[AnimationType.Hit]) return AnimationType.Hit;
    //    if (animCond[AnimationType.RecoveryFromHit])
    //    {
    //        animCond[AnimationType.RecoveryFromHit] = false;
    //        return AnimationType.RecoveryFromHit;
    //    }

    //    if (animCond[AnimationType.DefendHit]) return AnimationType.DefendHit;
    //    if (animCond[AnimationType.DefendEnd]) return AnimationType.DefendEnd;
    //    if (animCond[AnimationType.DefendStart])
    //    {
    //        //character.OnBlockActive?.Invoke(this, EventArgs.Empty);
    //        animCond[AnimationType.DefendStart] = false;
    //        return AnimationType.DefendStart;
    //    }
    //    if (animCond[AnimationType.DefendLoop]) return AnimationType.DefendLoop;

    //    if (animationData.optionIsHeld)
    //    {
    //        if (animCond[AnimationType.CharacterOptionEnd]) return AnimationType.CharacterOptionEnd;
    //        if (animCond[AnimationType.CharacterOptionStart]) return AnimationType.CharacterOptionStart;
    //        if (animCond[AnimationType.CharacterOptionLoop]) return AnimationType.CharacterOptionLoop;
    //    }

    //    if (animationData.optionIsTriggered)
    //    {
    //        if (animCond[AnimationType.CharacterOptionStart]) return AnimationType.CharacterOptionStart;
    //    }

    //    if (animCond[AnimationType.Roll]) return AnimationType.Roll;
    //    if (animCond[AnimationType.JumpEnd]) return AnimationType.JumpEnd;
    //    if (animCond[AnimationType.JumpStart]) return AnimationType.JumpStart;

    //    if (animCond[AnimationType.Ultimate]) return AnimationType.Ultimate;
    //    if (animCond[AnimationType.Attack1]) return AnimationType.Attack1;
    //    if (animCond[AnimationType.Attack2]) return AnimationType.Attack2;
    //    if (animCond[AnimationType.Attack3]) return AnimationType.Attack3;

    //    //if (character.grounded) return character.Rb.velocity.x == 0 ? AnimationType.Idle : AnimationType.Run;
    //    if (animCond[AnimationType.JumpAttack]) return AnimationType.JumpAttack;
    //    if (character.Rb.velocity.y > 3f) return AnimationType.JumpRising;
    //    if (character.Rb.velocity.y > 1f) return AnimationType.JumpPeak;
    //    return character.Rb.velocity.y < -1f ? AnimationType.JumpFalling : AnimationType.JumpRising;
    //}

    void ChangeFaceDirection(Vector2 v)
    {//{
    //    if (v == Vector2.zero || !grounded) return;
    //    if (currentAnimation != 0) { if (!CanChangeDirection(currentAnimation)) return; }
    //    spriteRenderer.flipX = v.x <= 0;
    //    character.OnChangeFaceDirection?.Invoke(this, spriteRenderer.flipX);
    }

    bool CanChangeDirection(AnimationType t)
    {
        return animationCanChangeFaceDirection[t];
    }
}