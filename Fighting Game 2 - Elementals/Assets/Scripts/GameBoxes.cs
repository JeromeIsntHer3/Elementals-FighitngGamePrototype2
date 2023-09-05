using System.Collections.Generic;
using UnityEngine;

public class GameBoxes : MonoBehaviour
{
    public bool DrawGizmo = true;
    public List<Collider2D> colliders;
    public Color color;
    protected Vector3[] points;

    void Awake()
    {
        foreach (var col in colliders)
        {
            if(!col.TryGetComponent(out GameBox gb)) return;
            gb.SetOwner(transform.root.GetComponent<BaseCharacter>());
        }
    }

    void OnDrawGizmos()
    {
        if(!DrawGizmo) return;
        if(colliders.Count <= 0) return;
        Gizmos.color = color;
        foreach (var collider in colliders)
        {
            points = new Vector3[]
            {
                new Vector3 (collider.bounds.min.x, collider.bounds.max.y, 0),
                new Vector3 (collider.bounds.max.x, collider.bounds.max.y, 0),
                new Vector3 (collider.bounds.max.x, collider.bounds.min.y, 0),
                new Vector3 (collider.bounds.min.x, collider.bounds.min.y, 0)
            };

            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[0]);

            //Gizmos.DrawLineList(points);
        }
    }
}