using System.Collections.Generic;
using System.Xml.Serialization;
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

        public void CreateChildBranch(TreeBranchData data)
        {
            //ToDo: Actual Functionality of creating the new branch.
        }

        public void HandleSliceReparenting(GameObject[] slicedPieces, float sliceDistanceFromRoot)
        {
            //Todo: Rechild children to each appropriate part.

            //Todo: Find sliced piece to remain attached, and replace the old one it with the new one.

            //ToDo: Add Rigidbody to unattached piece, and destroy it after a period of time. 
        }

    }
}