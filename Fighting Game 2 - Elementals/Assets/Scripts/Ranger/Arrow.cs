using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public class ArrowData
    {
        public bool FlipSprite;
        public Vector2 FlightDirection;
        public float FlightSpeed;
        public float Lifespan;

        public Vector2 FlightPath()
        {
            return FlightSpeed * FlightDirection;
        }
    }

    Rigidbody2D rb;
    SpriteRenderer sr;
    float deathTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (deathTime < Time.time) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void Spawn(ArrowData data)
    {
        sr.flipX = data.FlipSprite;
        rb.AddForce(data.FlightPath(), ForceMode2D.Impulse);
        deathTime = Time.time + data.Lifespan;
        float angle = Mathf.Atan2(data.FlightDirection.y, data.FlightDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}