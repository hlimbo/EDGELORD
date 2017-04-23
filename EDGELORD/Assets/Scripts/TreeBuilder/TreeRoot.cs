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
        public TreeBranch StartBranch;
        public TreeBranch CurrentHoverBranch;

        public void Awake()
        {
            CurrentHoverBranch = StartBranch;
            BranchList.Add(StartBranch);
        }
        public void CreateBranch(TreeBranchData data)
        {
            //ToDo: CreateBranch From Data
            //data.ParentBranch.CreateChildBranch(data);
            var go = GameObject.Instantiate(BranchPrefab, data.LocalBasePoint, Quaternion.identity);
            TreeBranch branch = go.GetComponent<TreeBranch>();
            branch.Generate(data);


            BranchList.Add(branch);
        }
    }
}