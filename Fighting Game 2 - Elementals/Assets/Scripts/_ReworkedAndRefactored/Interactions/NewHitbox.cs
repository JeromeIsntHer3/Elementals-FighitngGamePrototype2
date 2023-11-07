using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHitbox : MonoBehaviour
{
    [SerializeField] bool drawBox;
    [SerializeField] Color boxDrawnColor;
    Vector3[] points;

    [SerializeField] protected GameObject OwnerObject;
    [SerializeField] Collider2D hitboxCollider;

    public Collider2D HitboxCollider {  get { return hitboxCollider; } }

    void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
    }

    void OnDrawGizmos()
    {
        if (drawBox)
        {
            Gizmos.color = boxDrawnColor;
            points = new Vector3[]
            {
                new Vector3 (hitboxCollider.bounds.min.x, hitboxCollider.bounds.max.y, 0),
                new Vector3 (hitboxCollider.bounds.max.x, hitboxCollider.bounds.max.y, 0),
                new Vector3 (hitboxCollider.bounds.max.x, hitboxCollider.bounds.min.y, 0),
                new Vector3 (hitboxCollider.bounds.min.x, hitboxCollider.bounds.min.y, 0)
            };

            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[0]);
        }
    }
}