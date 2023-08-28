using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBAttacks : BaseCharacterAttacks
{
    [SerializeField] MBKnife knifePrefab;
    [SerializeField] Transform knifeThrowSpawn;
    [SerializeField] Transform trapThrowSpawn;

    [Header("Values")]
    [SerializeField] float knifeSpeed;
    [SerializeField] float trapSpeed;
 
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ThrowKnife() 
    {
        Vector2 faceDir = IsFacingLeft ? Vector2.left : Vector2.right;
        var knife = Instantiate(knifePrefab, knifeThrowSpawn.position, Quaternion.identity);
        knife.SetupKnife(character, GetDamageData(AttackType.Two),5f, IsFacingLeft,
            faceDir.normalized,knifeSpeed, false);
    }

    public void ThrowTrap()
    {
        Vector2 faceDir = trapThrowSpawn.position - centre.position;

        var knife = Instantiate(knifePrefab, trapThrowSpawn.position, Quaternion.identity);
        knife.SetupKnife(character, GetDamageData(AttackType.Two), 5f, IsFacingLeft,
            faceDir, trapSpeed, true);
    }
}