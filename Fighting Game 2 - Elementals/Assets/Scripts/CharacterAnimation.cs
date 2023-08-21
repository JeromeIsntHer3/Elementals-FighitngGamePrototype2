using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAnimation
{
    [HideInInspector] public string dispName;
    public AnimationType Type;
    public AnimationClip Clip;
    public bool canChangeFaceDirection;
}