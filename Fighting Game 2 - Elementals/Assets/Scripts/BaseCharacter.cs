using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected CharacterAnimationSO animationData;
    [SerializeField] protected BaseCharacter enemy;

    protected readonly Dictionary<AnimationType, float> animationDuration = new();
    float recoveryTime;

    public virtual void Awake()
    {
        animationData.AddToDuration(animationDuration);
    }

    public float GetDuration(AnimationType t)
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

    public Vector2 EnemyDirection()
    {
        return (enemy.transform.position - transform.position).normalized;
    }
}