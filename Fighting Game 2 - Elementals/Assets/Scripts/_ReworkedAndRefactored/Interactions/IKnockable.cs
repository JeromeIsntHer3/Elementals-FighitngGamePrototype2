using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockable
{
    Rigidbody2D ObjectRigidbody { get; set; }
    void Knockback(Vector2 direction, float force);
}