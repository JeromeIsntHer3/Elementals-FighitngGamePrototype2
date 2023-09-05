using UnityEngine;

public class MBKnife : BaseProjectile
{
    bool trap;
    bool landed;
    bool enhanced;
    Animator anim;

    void OnEnable()
    {
        BeforeGuardCheck = BeforeGuard;
        AfterMainTrigger = AfterTrigger;
    }

    void OnDisable()
    {
        BeforeGuardCheck = null;
        AfterMainTrigger = null;
    }

    public void SetupKnife(BaseCharacter owner, DamageData data, float lifespan, bool flipX,
        Vector2 direction, float speed, bool isTrap = false, bool enhanced = false)
    {
        InitProjectile(owner, data, lifespan, false);

        trap = isTrap;
        this.enhanced = enhanced;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
        anim = GetComponent<Animator>();
        Invoke(nameof(Explode), lifespan / 2);
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y,rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Explode()
    {
        anim.CrossFade("KnifeExplode", 0, 0);
    }

    void BeforeGuard(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;
        Vector2 landingPoint;
        if (enhanced)
        {
            landingPoint = GetTeleportPosition(hurtbox.transform);
            landingPoint = new Vector2(landingPoint.x, landingPoint.y + (owner.IsFacingLeft ? 1 : -1));
            owner.transform.position = landingPoint;
        }
    }

    void AfterTrigger(Collider2D col)
    {
        Vector2 landingPoint;

        if(col.TryGetComponent(out Hitbox hitbox))
        {
            if (BelongsToOwner(hitbox)) return;
            hitSomething = true;

            if(hitbox.CanDeflect)
            {
                Vector2 currVel = rb.velocity;
                rb.velocity = Vector2.zero;
                Vector2 dir = currVel.x > 0 ? Vector2.left : Vector2.right;
                rb.AddForce(dir * currVel * 3, ForceMode2D.Impulse);
                hitSomething = false;
                owner = hitbox.BoxOwner;
                return;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        landingPoint = GetTeleportPosition(col.transform);
        if (enhanced)
        {
            owner.transform.position = new Vector2(landingPoint.x, landingPoint.y + 1.9f);
            Destroy(gameObject);
        }
        else if (trap)
        {
            if (landed) return;
            anim.CrossFade("Knife_Land", 0, 0);
            transform.position = landingPoint;
            transform.eulerAngles = Vector3.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            landed = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Vector2 GetTeleportPosition(Transform objectHit)
    {
        return GetComponent<Collider2D>().ClosestPoint(objectHit.transform.position);
    }
}