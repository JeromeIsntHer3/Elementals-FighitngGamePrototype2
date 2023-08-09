using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovement", menuName = "Character/PhysicsData")]
public class CharacterMovementData : ScriptableObject
{
    public float PlayerSpeed;
    public float AccelerationSpeed;
    public float JumpForce;
    public float JumpDelay;
    public float DashForce;
    public int JumpsAllowed;
    public float SlideMultiplier;
}