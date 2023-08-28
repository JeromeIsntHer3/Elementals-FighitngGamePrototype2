using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCrystalProjectile : BaseProjectile
{
    Animator anim;

    readonly int LoopHash = Animator.StringToHash("Crystal_Moving");
    readonly int EndHash = Animator.StringToHash("Crystal_End");

    Vector3 spawnPosition;

    void OnEnable()
    {
        anim = GetComponent<Animator>();
        OnTriggerEvent += OnTrigger;
    }

    void OnDisable()
    {
        OnTriggerEvent -= OnTrigger;
    }

    public void SetupCrystal(BaseCharacter owner, DamageData data, float time, bool flipX, Vector2 direction, float speed)
    {
        InitProjectile(owner, data, time, flipX);

        rb.velocity = direction * speed;
        transform.localScale = new Vector2(1.5f, 1.5f);
        anim.CrossFade(LoopHash, 0, 0);
        spawnPosition = transform.position;
    }

    void Update()
    {
        if (TravelledMaxDistance()) Destroy(gameObject);
    }

    void OnTrigger(object sender, Collider2D collision)
    {
        if (!collision.transform.root.TryGetComponent(out CMRockProjectile rock)) return;
        anim.CrossFade(EndHash, 0,0);
        rb.velocity = Vector2.zero;
        transform.localScale = new Vector2(1f, 1f);

        rock.Explode();
        Invoke(nameof(DestroyCrystal), .1f);
    }

    bool TravelledMaxDistance()
    {
        return (transform.position - spawnPosition).magnitude > 3.5f;
    }

    void DestroyCrystal()
    {
        Destroy(gameObject);
    }
}