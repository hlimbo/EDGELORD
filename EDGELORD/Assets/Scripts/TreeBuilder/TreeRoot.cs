using System;
using System.Collections.Generic;
using Players;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class TreeRoot : MonoBehaviour, IBranchable
    {
        public PlayerID OwningPlayer;
        public Color BranchTint;
        public List<TreeBranch> BranchList = new List<TreeBranch>();
        public GameObject BranchPrefab;
        public TreeBranch StartBranch;
        public TreeBranch CurrentHoverBranch;
        
        public Action<Vector3> OnBranchCutAction = delegate { };
        public Action OnUpdateTreeCollider = delegate { };
        public Action<float> OnUpdateArea = delegate { };

        public float TotalArea { get; private set; }

        private int branchSortOrderIndex = 0;

        public void Awake()
        {
            CurrentHoverBranch = StartBranch;
            StartBranch.SetTint(BranchTint, branchSortOrderIndex);
            BranchList.Add(StartBranch);
            OnBranchCutAction += vector3 => { OnUpdateTreeCollider(); UpdateTotalArea(); Debug.Log("Area!"); };
            UpdateTotalArea();
        }
        public TreeBranch CreateBranch(TreeBranchData data)
        {
            //ToDo: CreateBranch From Data
            var go = GameObject.Instantiate(BranchPrefab, data.LocalBasePoint, Quaternion.identity);
            TreeBranch branch = go.GetComponent<TreeBranch>();
            branch.Generate(data);
            branch.SetTint(BranchTint, ++branchSortOrderIndex);
            branch.OwningPlayer = OwningPlayer;

            data.ParentBranch.AddChildBranch(branch);
            BranchList.Add(branch);
            return branch;
        }

        public void OnCompleteBranchCallback()
        {
            OnUpdateTreeCollider();
            UpdateTotalArea();
        }

        public void SetTotalArea(float a)
        {
            TotalArea = a;
        }

        public void UpdateTotalArea()
        {
            OnUpdateArea(TotalArea);
        }
    }
}