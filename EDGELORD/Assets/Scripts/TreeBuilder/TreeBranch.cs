using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework.Internal.Execution;
using Players;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    /// <summary>
    /// Branch Component that goes onto each branch. 
    /// Each Branch should have it's root transform at the base of the branch.
    /// 
    /// - Creates Child Branches
    /// - Handles Slicing
    /// - Sends Slice to enemy branches.
    /// </summary>
    public class TreeBranch : MonoBehaviour, IBranchable
    {
        public PlayerID OwningPlayer;
        public List<TreeBranch> DirectChildBranches = new List<TreeBranch>();
        [Space]
        public GameObject SpriteObject; // The visual 
        public TreeBranchData BranchData;

        public TreeRoot MyRoot;

        private void Start()
        {
            var roots = FindObjectsOfType<TreeRoot>();
            foreach (TreeRoot root in roots)
            {
                if (root.OwningPlayer == OwningPlayer) MyRoot = root;
            }
            if (MyRoot == null)
            {
                Debug.LogWarning("[TreeBranch] Matching Root not found!");
                MyRoot = roots[0];
            }
        }
        // Call this when the Branch is created.
        public void Generate(TreeBranchData data)
        {
            BranchData = data;
            //ToDo: Actually Generate Self based on input data.
            this.transform.parent = data.ParentBranch.transform;
            this.transform.localPosition = data.LocalBasePoint;
            var rot = Quaternion.LookRotation(Vector3.forward, (Vector3)data.GrowDirection.normalized);
            this.transform.rotation = rot;
        }

        public void SliceBranch(Vector3 worldStartPoint, Vector3 worldEndPoint, GameObject cutGameObject)
        {
            List<SpriteSlicer2DSliceInfo> sliceInfoList = new List<SpriteSlicer2DSliceInfo>();
            SpriteSlicer2D.SliceSprite(worldStartPoint, worldEndPoint, cutGameObject, false, ref sliceInfoList);
            
            HandleSliceReparenting(sliceInfoList.ToArray());

        }

        public void HandleSliceReparenting(SpriteSlicer2DSliceInfo[] sliceInfo)
        {
            //GameObject[] slicedPieces = 
            //float sliceDistanceFromRoot = 
            //Todo: Rechild children to each appropriate part.

            //Todo: Find sliced piece to remain attached, and replace the old one it with the new one.

            //ToDo: Add Rigidbody to unattached piece, and destroy it after a period of time. 
        }
    }
}