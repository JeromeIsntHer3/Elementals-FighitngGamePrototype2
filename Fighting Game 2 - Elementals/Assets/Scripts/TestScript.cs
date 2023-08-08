using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject player;
    public List<string> animationNames;

    Animator animator;
    List<int> animationHashes = new ();

    void Start()
    {
        animator = player.GetComponent<Animator>();

        foreach(string anim in animationNames)
        {
            int hashed = Animator.StringToHash(anim);
            animationHashes.Add(hashed);
        }
    }

    void Update()
    {
        
    }
}