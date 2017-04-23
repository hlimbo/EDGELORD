using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EDGELORD
{
	public class CompositeColliderTester : MonoBehaviour
	{
	    public CompositeCollider2D cc2d;
        public KeyCode testCode = KeyCode.Alpha0;

	    private void Update()
	    {
	        if (Input.GetKeyDown(testCode))
	        {
	            Debug.Log(cc2d.pointCount);
	        }
	    }
	}
}