using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Hitbox
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    float deathTime;

    void Update()
    {
        if (deathTime < Time.time) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        Destroy(gameObject);
    }

    public void SetupArrow(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan)
    {
        base.SetDamageData(data, owner);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = flipX;
        rb.AddForce(dir * speed,ForceMode2D.Impulse);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        deathTime = Time.time + lifespan;
    }
}