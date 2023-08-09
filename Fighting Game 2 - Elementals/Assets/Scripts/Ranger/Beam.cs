using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public class BeamData
    {
        public bool FlipSprite;
        public float Lifespan;
    }

    SpriteRenderer sr;
    float deathTime;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Spawn(BeamData data)
    {
        sr.flipX = data.FlipSprite;
        //deathTime = Time.time + data.Lifespan;
    }

    void Update()
    {
        //if (deathTime < Time.time) Destroy(gameObject);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}