using System.Collections.Generic;
using Players;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class TreeRoot : MonoBehaviour, IBranchable
    {
        public PlayerID OwningPlayer;
        public List<TreeBranch> BranchList = new List<TreeBranch>();
        public GameObject BranchPrefab;
        
        public void CreateBranch(TreeBranchData data)
        {
            //ToDo: CreateBranch From Data
            data.ParentBranch.CreateChildBranch(data);
        }

    }
}