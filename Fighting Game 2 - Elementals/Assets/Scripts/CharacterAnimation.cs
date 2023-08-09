using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAnimation
{
    public AnimationType Type;
    public AnimationClip Clip;
    public bool canChangeFaceDirection;
    public bool canBeInterrupted;
}