using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class CompositeColliderBuilder : MonoBehaviour
    {
        public TreeRoot treeRoot;
        public Transform treeStart;
        public KeyCode testKey;
        public PolygonCollider2D myCollider;
        public CompositeCollider2D myComposite;
        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                BuildCompositeCollider();
            }
        }

        private void BuildCompositeCollider()
        {
            var colliders = treeStart.GetComponentsInChildren<PolygonCollider2D>();
            List<Vector2> AllPoints = new List<Vector2>();
            foreach (PolygonCollider2D c in colliders)
            {
                foreach (Vector2 point in c.points)
                {
                    AllPoints.Add(myCollider.transform.InverseTransformPoint(c.transform.TransformPoint(point)));
                }
                //AllPoints.AddRange(c.points);

            }
            myCollider.points = AllPoints.ToArray();
            Debug.Log(myComposite.pointCount);

            var area = Area(ref AllPoints);
            Debug.Log(area);

        }
        // Get the area of the given polygon
        public static float Area(ref List<Vector2> points)
        {
            int n = points.Count;
            float A = 0.0f;

            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector2 pval = points[p];
                Vector2 qval = points[q];
                A += pval.x * qval.y - qval.x * pval.y;
            }

            return (A * 0.5f);
        }
    }
}