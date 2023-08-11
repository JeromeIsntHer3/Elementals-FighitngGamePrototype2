using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public class ArrowData : DamageData
    {
        public bool FlipSprite;
        public Vector2 FlightDirection;
        public float FlightSpeed;
        public float Lifespan;
        public BaseCharacter Owner;

        public Vector2 FlightPath()
        {
            return FlightSpeed * FlightDirection;
        }
    }

    Rigidbody2D rb;
    SpriteRenderer sr;
    float deathTime;
    float damage;
    float hKb;
    float vKb;
    BaseCharacter owner;
    DamageType damageType;

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

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.TryGetComponent(out Hurtbox hurtbox))
        {
            if (hurtbox.CheckHitOwner(owner)) return;
            Vector2 attackDir = hurtbox.transform.root.position - owner.transform.position;
            hurtbox.Hit(new DamageData
            {
                Damage = damage,
                Direction = attackDir,
                HorizontalKnockback = hKb,
                VerticalKnockback = vKb,
                Type = damageType
            });
        }
        Destroy(gameObject);
    }

    public void Spawn(ArrowData arrowData)
    {
        sr.flipX = arrowData.FlipSprite;
        rb.AddForce(arrowData.FlightPath(), ForceMode2D.Impulse);
        deathTime = Time.time + arrowData.Lifespan;
        float angle = Mathf.Atan2(arrowData.FlightDirection.y, arrowData.FlightDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        damage = arrowData.Damage;
        owner = arrowData.Owner;
        vKb = arrowData.VerticalKnockback;
        hKb = arrowData.HorizontalKnockback;
        damageType = arrowData.Type;
    }
}