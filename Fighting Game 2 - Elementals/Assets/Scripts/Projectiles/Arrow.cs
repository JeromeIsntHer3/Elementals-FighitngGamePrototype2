using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BaseProjectile
{
    void OnEnable()
    {
        AfterMainTrigger = AfterTrigger;
    }

    void OnDisable()
    {
        AfterMainTrigger = null;
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void AfterTrigger(Collider2D col)
    {
        Destroy(gameObject);
    }

    public void SetupArrow(DamageData data, BaseCharacter owner, bool flipX, Vector2 dir, float speed, float lifespan)
    {
        base.InitProjectile(owner, data, lifespan, false);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.AddForce(dir * speed,ForceMode2D.Impulse);
    }
}