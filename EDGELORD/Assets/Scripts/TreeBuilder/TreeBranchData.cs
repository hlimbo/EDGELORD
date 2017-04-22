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

    }
}