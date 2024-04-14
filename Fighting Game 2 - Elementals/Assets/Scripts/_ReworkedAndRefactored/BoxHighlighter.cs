using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHighlighter : MonoBehaviour
{
    enum HighlightType
    {
        Box,
        Circle
    }

    [SerializeField] HighlightType type;
    [SerializeField] bool drawBox;
    [SerializeField] Color boxDrawnColor;
    Vector3[] points;

    [SerializeField] Collider2D box;
    [SerializeField] Transform highlightOrigin;
    [SerializeField] float highlightRadius;

    void Awake()
    {
        box = GetComponent<Collider2D>();
    }

    void OnDrawGizmos()
    {
        if (!drawBox) return;

        Gizmos.color = boxDrawnColor;
        switch (type)
        {
            case HighlightType.Box:

                points = new Vector3[]
                {
                new(box.bounds.min.x, box.bounds.max.y, 0),
                new (box.bounds.max.x, box.bounds.max.y, 0),
                new  (box.bounds.max.x, box.bounds.min.y, 0),
                new (box.bounds.min.x, box.bounds.min.y, 0)
                };

                Gizmos.DrawLine(points[0], points[1]);
                Gizmos.DrawLine(points[1], points[2]);
                Gizmos.DrawLine(points[2], points[3]);
                Gizmos.DrawLine(points[3], points[0]);

                break;


            case HighlightType.Circle:

                Gizmos.DrawWireSphere(highlightOrigin.transform.position, highlightRadius);

                break;
        }
    }
}