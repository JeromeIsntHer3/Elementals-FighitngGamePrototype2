using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IHitbox
{
    public class ArrowData : DamageData
    {
        public bool FlipSprite;
        public Vector2 FlightDirection;
        public float FlightSpeed;
        public float DeathTime;

        public Vector2 FlightPath()
        {
            return FlightSpeed * FlightDirection;
        }
    }

    Rigidbody2D rb;
    SpriteRenderer sr;
    ArrowData arrowData;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (arrowData.DeathTime < Time.time) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out BlockPredictionBox blockBox)) return;
        Destroy(gameObject);
    }

    public void SpawnArrow(ArrowData arrowData)
    {
        sr.flipX = arrowData.FlipSprite;
        rb.AddForce(arrowData.FlightPath(), ForceMode2D.Impulse);
        float angle = Mathf.Atan2(arrowData.FlightDirection.y, arrowData.FlightDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.arrowData = arrowData;
    }

    public DamageData Data()
    {
        return arrowData;
    }
}