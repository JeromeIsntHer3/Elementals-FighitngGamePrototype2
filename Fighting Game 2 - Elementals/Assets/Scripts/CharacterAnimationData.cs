using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimation", menuName = "Character/AnimationData")]
public class CharacterAnimationData : ScriptableObject
{
    public List<CharacterAnimation> CharacterAnimations = new();

    public void AddToDicts(BaseCharacterAnimator.AnimationTransferData data)
    {
        foreach (CharacterAnimation animation in CharacterAnimations)
        {
            int animHash = Animator.StringToHash(animation.Clip.name);
            data.hashes.Add(animation.Type, animHash);
            data.durations.Add(animation.Type, animation.Clip.averageDuration);
            data.canFlipX.Add(animHash, animation.canChangeFaceDirection);
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