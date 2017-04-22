using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicingTests : MonoBehaviour {


    [SerializeField]
    Vector3 startPoint;//location of where gameObject with this attached script.

    [SerializeField]
    Vector3 endPoint;

    [SerializeField]
    List<SpriteSlicer2DSliceInfo> slicedObjectInfo = null;

    public GameObject spriteToSlice;

    public bool canDestroyParent;

    [SerializeField]
    bool isMousePressed = false;

    public LayerMask slicableMask;


    //weird side task, but I'm trying to write code to visually see where a slice happens.. may not be relevant to build
	void Start ()
    {
        SpriteSlicer2D.DebugLoggingEnabled = true;
        startPoint = transform.position;
	}

    //SpriteSlicer2D.SliceSprite(startPoint, endPoint, spriteToSlice, canDestroyParent, ref slicedObjectInfo);
    void Update ()
    {

        //A crude function to slice objects 
        //if (Input.GetMouseButton(0))
        //{
        //    if(!isMousePressed)
        //    {
        //        isMousePressed = true;
        //        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    }
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.DrawLine(startPoint, endPoint, Color.red, 5.0f);
        //    isMousePressed = false;
        //}


        //THIS ONE PLS
        if (Input.GetMouseButton(0))
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(startPoint, endPoint, Color.red);
        }

        if(Input.GetMouseButtonUp(0))
        {
            SpriteSlicer2D.SliceAllSprites(startPoint, endPoint, canDestroyParent, ref slicedObjectInfo, slicableMask);
        }

    }

}
