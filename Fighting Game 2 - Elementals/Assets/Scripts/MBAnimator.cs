using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBAnimator : BaseCharacterAnimator
{
    [SerializeField] float invisDuration;

    MBAttacks attacks;
    bool invisible;
    float invisbleTilTime;

    void Start()
    {
        OptionPerformCondition = BackflipCondition;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        attacks = GetComponent<MBAttacks>();
        attacks.OnInvicibleStateChanged += SetInvisible;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        attacks.OnInvicibleStateChanged -= SetInvisible;
    }

    protected override void Update()
    {
        if (invisible)
        {
            if (Time.time > invisbleTilTime) attacks.OnInvicibleStateChanged?.Invoke(this, false);
        }
        base.Update();
    }

    bool BackflipCondition()
    {
        return true;
    }

    void SetInvisible(object sender, bool invisble)
    {
        invisible = invisble;
        if (invisble)
        {
            SetVisibility(new Color32(255, 255, 255, 50));
            invisbleTilTime = Time.time + invisDuration;
        }
        else
        {
            SetVisibility(new Color32(255, 255, 255, 255));
        }
    }

    void SetVisibility(Color32 color)
    {
        spriteRenderer.color = color;
    }
}