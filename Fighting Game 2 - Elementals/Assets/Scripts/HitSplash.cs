using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSplash : MonoBehaviour
{
    float despawnTilTime;

    public void Setup(float t, bool flip)
    {
        despawnTilTime = Time.time + t;
        GetComponent<SpriteRenderer>().flipX = flip;
    }

    void Update()
    {
        if (Time.time > despawnTilTime) Destroy(gameObject);
    }
}