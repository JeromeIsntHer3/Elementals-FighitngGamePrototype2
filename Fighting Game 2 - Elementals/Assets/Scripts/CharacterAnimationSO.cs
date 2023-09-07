using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimation", menuName = "Character/AnimationData")]
public class CharacterAnimationSO : ScriptableObject
{
    public bool optionIsHeld;
    public bool optionIsTriggered;
    public List<CharacterAnimation> CharacterAnimations = new();

    public void AddToHashesDict(Dictionary<AnimationType, int> hashDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            int hash = Animator.StringToHash(animation.Clip.name);
            hashDict.Add(animation.Type, hash);
        }
    }

    public void InitializeHashes()
    {
        foreach (CharacterAnimation animation in CharacterAnimations)
        {
            int hash = Animator.StringToHash(animation.Clip.name);
            animation.SetHash(hash);
        }
    }

    public void AddToCanChangeDirectionDict(Dictionary<AnimationType, bool> changeDirectionDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            changeDirectionDict.Add(animation.Type, animation.CanChangeFaceDirection);
        }
    }

    public void AddToConditionList(Dictionary<AnimationType, bool> conds)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            conds.Add(animation.Type, animation.AnimationCondition);
        }
    }

    public void AddToFullyAnimate(Dictionary<AnimationType, bool> fullyAnimateDict)
    {
        foreach(CharacterAnimation animation in CharacterAnimations)
        {
            fullyAnimateDict.Add(animation.Type, animation.IsFullyAnimated);
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