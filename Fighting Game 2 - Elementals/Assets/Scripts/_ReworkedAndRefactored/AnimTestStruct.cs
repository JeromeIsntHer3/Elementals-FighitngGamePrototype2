using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestStruct : MonoBehaviour
{
    public List<AnimStruct> animations = new();
    public List<CharacterAnimation> aniamtions;


    void Awake()
    {
        if (aniamtions[0] == aniamtions[1])
        {

        }
    }
}


[Serializable]
public struct AnimStruct
{
    public AnimationType Type;
    public AnimationClip Clip;
    public bool AnimationCondition;
    public bool IsFullyAnimated;
    public bool CanChangeFaceDirection;

    public int AnimationHash { get; private set; }

    public void SetHash(int hash)
    {
        AnimationHash = hash;
    }
}