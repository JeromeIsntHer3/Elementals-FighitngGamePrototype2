using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBox : MonoBehaviour
{
    [SerializeField] protected BaseCharacter owner;
    protected Collider2D thisCol;
    public BaseCharacter BoxOwner { get { return owner; } }

    void Awake()
    {
        thisCol = GetComponent<Collider2D>();
    }

    public void SetOwner(BaseCharacter owner)
    {
        this.owner = owner;
    }
}