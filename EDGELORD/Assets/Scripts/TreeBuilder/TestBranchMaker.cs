using System.Collections;
using System.Collections.Generic;
using EDGELORD.TreeBuilder;
using UnityEngine;

namespace EDGELORD
{
	public class TestBranchMaker : MonoBehaviour
	{
        public GameObject BranchPrefab;
	    public TreeRoot TreeRoot;
	    public TreeBranchData BranchData;
	    public KeyCode TestInput = KeyCode.Space;

	    private void Update()
	    {
	        if (Input.GetKeyDown(TestInput))
	        {
	            GenerateBranch(BranchData);
	        }
	    }

	    public void GenerateBranch(TreeBranchData data)
	    {
            TreeRoot.CreateBranch(data);
	    }
	}
}