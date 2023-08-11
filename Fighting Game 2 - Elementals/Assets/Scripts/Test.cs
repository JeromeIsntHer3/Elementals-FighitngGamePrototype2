using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float pushforce = 5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.root.TryGetComponent(out BaseCharacter other))
        {
            Vector2 pushDir = other.transform.position - rb.transform.position;

            rb.AddForce(pushDir.normalized* pushforce, ForceMode2D.Impulse);
        }
    }
}
