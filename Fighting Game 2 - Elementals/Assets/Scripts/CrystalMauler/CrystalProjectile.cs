using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalProjectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;

    int StartHash = Animator.StringToHash("Crystal_Start");
    int LoopHash = Animator.StringToHash("Crystal_Moving");
    int EndHash = Animator.StringToHash("Crystal_End");
    int BoolHash = Animator.StringToHash("Hit");

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetupCrystal(float speed, Vector2 dir, float time)
    {
        rb.velocity = dir * speed;

        Invoke(nameof(SelfDestruct), time);
        anim.CrossFade(LoopHash, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out GroundProjectile projectile))
        {
            anim.SetBool(BoolHash, true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out GroundProjectile projectile))
        {
            anim.SetBool(BoolHash, true);
        }
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}