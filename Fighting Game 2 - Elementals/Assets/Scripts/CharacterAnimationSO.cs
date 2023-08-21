using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimation", menuName = "Character/AnimationData")]
public class CharacterAnimationSO : ScriptableObject
{
    public List<CharacterAnimation> CharacterAnimations = new();

    public void AddToHashesDict(Dictionary<AnimationType, int> hashDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            int hash = Animator.StringToHash(animation.Clip.name);
            hashDict.Add(animation.Type, hash);
        }
    }

    public void AddToCanChangeDirectionDict(Dictionary<AnimationType, bool> changeDirectionDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            changeDirectionDict.Add(animation.Type, animation.canChangeFaceDirection);
        }
    }

    public void AddToConditionDict(Dictionary<AnimationType, bool> conditionDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            conditionDict.Add(animation.Type, animation.AnimationCondition);
        }
    }

    public void AddToFullyAnimate(Dictionary<AnimationType, bool> fullyAnimateDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            fullyAnimateDict.Add(animation.Type, animation.FullyAnimate);
        }
    }

    public void AddToDuration(Dictionary<AnimationType, float> animTimes)
    {
        foreach (CharacterAnimation animation in CharacterAnimations)
        {
            animTimes.Add(animation.Type, animation.Clip.averageDuration);
        }
    }
}