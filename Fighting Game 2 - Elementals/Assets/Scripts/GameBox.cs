using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBox : MonoBehaviour
{
    [SerializeField] protected BaseCharacter owner;
    public BaseCharacter BoxOwner { get { return owner; } }

    public void SetOwner(BaseCharacter owner)
    {
        this.owner = owner;
    }
}