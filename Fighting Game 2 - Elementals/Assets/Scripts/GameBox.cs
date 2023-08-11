using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBox : MonoBehaviour
{
    [SerializeField] protected BaseCharacter owner;

    public void SetOwner(BaseCharacter owner)
    {
        this.owner = owner;
    }
}