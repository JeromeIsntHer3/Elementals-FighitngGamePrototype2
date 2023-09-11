using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] CharacterAnimationSO animationData;

    readonly Dictionary<AnimationType, bool> animationCanChangeFaceDirection = new();
    readonly Dictionary<AnimationType, CharacterAnimation> animations = new();
    readonly Dictionary<AnimationType, bool> animationCondition = new();
    readonly Dictionary<AnimationType, float> animationDuration = new();

    SpriteRenderer spriteRenderer;
    Animator animator;
    CharacterAnimation currentAnimation;
    float lockedTilTime, attackTilTime;

    public SpriteRenderer Sr { get { return spriteRenderer; } }

    void Awake()
    {
        //Add Animations Here
        animationData.InitializeHashes();
        animationData.AddToCanChangeDirectionDict(animationCanChangeFaceDirection);
        animationData.AddToConditionList(animationCondition);
        animationData.AddToDuration(animationDuration);

        foreach(var animation in animationData.CharacterAnimations)
        {
            animations.Add(animation.Type, animation);
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void SetDelay(float extend)
    {
        lockedTilTime = Time.time + extend;
    }

    public void SetAnimation(AnimationType type)
    {
        if (lockedTilTime > Time.time) return;
        var newAnimation = animations[type];
        if (currentAnimation == newAnimation) return;

        if (newAnimation.IsFullyAnimated) SetDelay(GetDuration(type));

        Debug.Log(type);
        animator.CrossFade(newAnimation.AnimationHash, 0, 0);
        currentAnimation = newAnimation;
    }

    public float GetDuration(AnimationType type)
    {
        return animationDuration[type];
    }

    public void ClearRecovery()
    {
        lockedTilTime = 0;
    }

    public void ClearAttackRecovery()
    {
        attackTilTime = 0;
    }

    public void SetAttackDuration(int index)
    {
        float duration = 0;
        switch (index)
        {
            case 0:
                duration = GetDuration(AnimationType.Attack1);
                break;
            case 1:
                duration = GetDuration(AnimationType.Attack2);
                break;
            case 2:
                duration = GetDuration(AnimationType.Attack3);
                break;
            case 3:
                duration = GetDuration(AnimationType.Ultimate);
                break;
            case 4:
                duration = GetDuration(AnimationType.JumpAttack);
                break;
        }

        attackTilTime = Time.time + duration;
    }

    public bool IsAttackCompleted()
    {
        return attackTilTime < Time.time;
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