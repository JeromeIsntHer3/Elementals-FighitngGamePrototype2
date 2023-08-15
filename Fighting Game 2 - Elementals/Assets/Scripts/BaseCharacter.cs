using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] int player;
    [SerializeField] protected CharacterAnimationSO animationData;
    protected BaseCharacter enemy;

    protected readonly Dictionary<AnimationType, float> animationDuration = new();
    float recoveryTime;

    public virtual void Awake()
    {
        animationData.AddToDuration(animationDuration);
    }

    public void Start()
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