using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladekeeperAttacks : BaseCharacterAttacks
{
    [SerializeField] GameObject knifePrefab;
    [SerializeField] Transform knifeThrowSpawn;
    [SerializeField] Transform trapThrowSpawn;

    [Header("Values")]
    [SerializeField] float knifeSpeed;
 
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
        knife.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, faceDir, knifeSpeed, 5f);
    }

    public void ThrowTrap()
    {
        Vector2 faceDir = trapThrowSpawn.position - centre.transform.position;
        var trap = Instantiate(knifePrefab, trapThrowSpawn.position, Quaternion.identity);
        trap.GetComponent<Arrow>().SetupArrow(GetDamageData(AttackType.Two), character,
            IsFacingLeft, faceDir, knifeSpeed * 3f, 5f);
    }
}