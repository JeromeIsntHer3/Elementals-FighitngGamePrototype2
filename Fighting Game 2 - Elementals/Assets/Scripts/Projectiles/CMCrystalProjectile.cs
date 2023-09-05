using UnityEngine;

public class CMCrystalProjectile : BaseProjectile
{
    [SerializeField] CMCrystalProjectile ownPrefab;

    Animator anim;
    bool isEnhanced;
    bool flipX;
    float cantCreateUntil;
    float creationDuration = .1f;
    bool isParent;

    readonly int LoopHash = Animator.StringToHash("Crystal_Moving");
    readonly int EndHash = Animator.StringToHash("Crystal_End");

    Vector3 spawnedPosition;

    void OnEnable()
    {
        anim = GetComponent<Animator>();
        BeforeMainTrigger = BeforeTrigger;
    }

    void OnDisable()
    {
        BeforeMainTrigger = null;
    }

    public void SetupCrystal(BaseCharacter owner, DamageData data, float time, bool flipX, Vector2 direction,
        float speed = 0, bool enhance = false, bool isParent = false)
    {
        InitProjectile(owner, data, time, flipX);

        if (isParent)
        {
            rb.velocity = direction * speed;
            transform.localScale = new Vector2(1.5f, 1.5f);
            anim.CrossFade(LoopHash, 0, 0);
            spawnedPosition = transform.position;
        }

        isEnhanced = enhance;
        this.flipX = flipX;
        this.isParent = isParent;
    }

    void Update()
    {
        if (TravelledMaxDistance()) Destroy(gameObject);
        if (isEnhanced && cantCreateUntil < Time.time && isParent)
        {
            var crystal = Instantiate(ownPrefab, transform.position, Quaternion.identity);
            crystal.SetupCrystal(owner, damageData, .3f, flipX, Vector2.zero, 0, true);
            cantCreateUntil = Time.time + creationDuration;
        }
    }

    void BeforeTrigger(Collider2D collision)
    {
        if (isParent)
        {
            if (!collision.transform.root.TryGetComponent(out CMRockProjectile rock)) return;
            anim.CrossFade(EndHash, 0, 0);
            rb.velocity = Vector2.zero;
            transform.localScale = new Vector2(1f, 1f);

            rock.Explode();
            Invoke(nameof(DestroyCrystal), .1f);
        }
        if (!isEnhanced) return;
    }

    bool TravelledMaxDistance()
    {
        return (transform.position - spawnedPosition).magnitude > 3.5f;
    }

    void DestroyCrystal()
    {
        Destroy(gameObject);
    }
}