using System;
using UnityEngine;

[Serializable]
public class CharacterAnimation
{
    public AnimationType Type;
    public AnimationClip Clip;
    public bool AnimationCondition;
    public bool IsFullyAnimated;
    public bool CanInterrupt;
    public bool CanChangeFaceDirection;

    public int AnimationHash { get; private set; }

    public void SetHash(int hash)
    {
        AnimationHash = hash;
    }
}