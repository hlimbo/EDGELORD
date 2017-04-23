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
                AllPoints.AddRange(c.points);
            }
            myCollider.points = AllPoints.ToArray();
        }
    }
}