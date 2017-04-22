using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    [System.Serializable]
    public struct TreeBranchData
    {
        public float Length;
        public float Width;
        public Vector2 GrowDirection;
        public TreeBranch ParentBranch;
        public Vector3 LocalBasePoint;

        public TreeBranchData(float length, float width, Vector2 growDir, TreeBranch parentBranch, Vector3 localBasePoint)
        {
            Length = length;
            Width = width;
            GrowDirection = growDir;
            ParentBranch = parentBranch;
            LocalBasePoint = localBasePoint;
        }
    }
}