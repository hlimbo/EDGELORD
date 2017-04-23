using System;
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

        public Action<Vector3> OnBranchCutAction = delegate { };

        public float TotalArea { get; private set; }


        public void Awake()
        {
            CurrentHoverBranch = StartBranch;
            BranchList.Add(StartBranch);
            UpdateTotalArea();
        }
        public void CreateBranch(TreeBranchData data)
        {
            //ToDo: CreateBranch From Data
            var go = GameObject.Instantiate(BranchPrefab, data.LocalBasePoint, Quaternion.identity);
            TreeBranch branch = go.GetComponent<TreeBranch>();
            branch.Generate(data);
            branch.OwningPlayer = OwningPlayer;

            data.ParentBranch.AddChildBranch(branch);
            BranchList.Add(branch);
            UpdateTotalArea();
        }

        public void UpdateTotalArea()
        {
            foreach (TreeBranch branch in BranchList)
            {
                
            }
        }
    }
}