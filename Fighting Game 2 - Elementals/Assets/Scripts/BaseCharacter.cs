using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected CharacterAnimationData animationData;

    protected readonly Dictionary<AnimationType, float> animationDuration = new();
    protected float recoveryTime;

    public virtual void Awake()
    {
        animationData.AddToDuration(animationDuration);
    }

    public virtual float GetDuration(AnimationType t)
    {
        return animationDuration[t];
    }

    public virtual void SetRecoveryDuration(float t)
    {
        recoveryTime = Time.time + t;
    }

    public virtual bool Recovered()
    {
        return Time.time > recoveryTime;
    }
}