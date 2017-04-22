﻿using System.Collections;
using System.Collections.Generic;
using EDGELORD.TreeBuilder;
using UnityEngine;

namespace EDGELORD
{
	public class TestBranchMaker : MonoBehaviour
	{
	    public GameObject BranchPrefab;
	    public TreeBranchData BranchData;
	    public KeyCode TestInput = KeyCode.Space;

	    private void Update()
	    {
	        if (Input.GetKeyDown(TestInput))
	        {
	            GenerateBranch(BranchData);
	        }
	    }

	    private void GenerateBranch(TreeBranchData data)
	    {
	        var go = GameObject.Instantiate(BranchPrefab, data.LocalBasePoint, Quaternion.identity, this.transform);

	    }
	}
}