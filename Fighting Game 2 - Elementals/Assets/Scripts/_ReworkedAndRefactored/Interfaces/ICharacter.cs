using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    void Damage(float dmgAmount);
}