using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "Character/AttackData")]
public class CharacterAttackSO : ScriptableObject
{
    public List<AttackData> Attacks;

    public void AddToDict(Dictionary<AnimationType, AttackData> dict)
    {
        foreach(var attack in Attacks)
        {
            dict.Add(attack.AnimationType,attack);
        }
    }
}