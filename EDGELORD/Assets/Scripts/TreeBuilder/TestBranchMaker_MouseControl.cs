using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EDGELORD.TreeBuilder
{
	public class TestBranchMaker_MouseControl : MonoBehaviour 
	{
	    public enum ControlState
	    {
	        Default,
            Aiming,
	    }

	    public ControlState controlState = ControlState.Default;
	    //public TreeBranch ParentBranch;
        public TreeRoot Root;
        private Vector3 startPos;
	    private Vector3 aimVector;
	    private TestBranchMaker branchMaker;
	    public TreeBranch currentTargetBranch;

	    private void Awake()
	    {
	        var bm = GetComponent<TestBranchMaker>();
	        if (bm) branchMaker = bm;
	    }

	    private void Update()
	    {

            switch (controlState)
	        {
	            case ControlState.Default:
	                if (Input.GetMouseButtonDown(0))
	                {
	                    if (TryGetStartPosition())
	                    {

                            controlState = ControlState.Aiming;
	                    }
	                }
	                break;
	            case ControlState.Aiming:
	                if (Input.GetMouseButton(0))
	                {
                        aimVector = GetAimVector(startPos);
	                }
	                else
	                {
	                    //Release
                        controlState = ControlState.Default;
	                    TreeBranchData newTreeBranchData = GenerateTreeBranchData();

	                    branchMaker.GenerateBranch(newTreeBranchData);
	                    startPos = Vector3.zero;
	                    aimVector = Vector3.zero;
                    }
	                break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
	    }

	    private TreeBranchData GenerateTreeBranchData()
	    {
	        TreeBranchData b = new TreeBranchData();
	        b.Length = aimVector.magnitude;
	        b.Width = 1f; // Apply "Golden ratio" with length?
	        b.GrowDirection = aimVector.normalized;
	        b.ParentBranch = currentTargetBranch;
	        b.LocalBasePoint = b.ParentBranch.transform.InverseTransformPoint(startPos);
	        return b;
	    }

	    private Vector3 GetAimVector(Vector3 vectorStartPos)
	    {
	        var currentPos = GetMouseWorldPosition();
            Debug.DrawLine(startPos, currentPos, Color.red);
	        return currentPos - vectorStartPos;
	    }

	    private bool TryGetStartPosition()
	    {
            //ToDo: Make sure this has selected a branch to work from.
            startPos = GetMouseWorldPosition();

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = Camera.main.transform.position.z;
            RaycastHit2D rayCastResult = Physics2D.Raycast(mouseWorldPosition, new Vector3(0, 0, 0), 0.0f);
            if (rayCastResult.rigidbody != null)
	        {
	            Debug.Log("Hit!" );
                Debug.Log(rayCastResult.rigidbody);
	            var b = rayCastResult.rigidbody.GetComponent<TreeBranch_Sprite>();
	            if (b)
	            {
	                currentTargetBranch = b.OwnerTreeBranch;
	                Root.CurrentHoverBranch = b.OwnerTreeBranch;
	            }
	        }
	        else
	        {
                Debug.Log("No Hit.");
            }
            return true;
	    }

	    private Vector3 GetMouseWorldPosition()
	    {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	        mouseWorldPosition.z = 0f;//Camera.main.transform.position.z;
	        return mouseWorldPosition;
	        //   var v3 = Input.mousePosition;
	        //var ray = Camera.main.ScreenPointToRay(v3);
	        //v3 = ray.GetPoint(offset); //offset = 10f
	        //return v3;
	    }

    }
}