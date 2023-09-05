using System;
using UnityEngine;

[Serializable]
public class CharacterAnimation
{
    public AnimationType Type;
    public AnimationClip Clip;
    public bool AnimationCondition;
    public bool FullyAnimate;
    public bool canChangeFaceDirection;
}