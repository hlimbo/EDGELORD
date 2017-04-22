using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicingTests : MonoBehaviour {


    [SerializeField]
    Vector3 startPoint;//location of where gameObject with this attached script.

    [SerializeField]
    Vector3 endPoint;

    [SerializeField]
    List<SpriteSlicer2DSliceInfo> slicedObjectInfo = new List<SpriteSlicer2DSliceInfo>();

    public GameObject spriteToSlice;

    public bool canDestroyParent;

    [SerializeField]
    bool isMousePressed = false;

    public LayerMask slicableMask;

    public GameObject m_swordBase;

    public GameObject childPivotPointPrefab;


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


        startPoint = transform.position;
        if (Input.GetMouseButton(0))
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(startPoint, endPoint, Color.red);
        }

        if(Input.GetMouseButtonUp(0))
        {
            
            SpriteSlicer2D.SliceAllSprites(startPoint, endPoint, canDestroyParent, ref slicedObjectInfo, slicableMask);

            //variables to pass into this function
            GameObject slicedObject = GetSlicedObjectClosestToBase(slicedObjectInfo, m_swordBase);
            if(slicedObject != null)
            {
                Debug.Log(slicedObject.name);
            }

            //if (slicedObjectInfo != null)
            //{
            //    //retrieve the 2nd child and apply gravity to it.
            //    Debug.Log("List Count: " + slicedObjectInfo.Count);

            //    slicedObjectInfo[0].ChildObjects[1].GetComponent<Rigidbody2D>().gravityScale = 5;
            //    slicedObjectInfo[0].ChildObjects[0].GetComponent<Rigidbody2D>().isKinematic = true;

            //    //Case 1: sliced object is the branch of the sword.
            //    SpriteSlicer2DSliceInfo branch = slicedObjectInfo[0];
            //    List<GameObject> slicedParts = branch.ChildObjects;//should only return 2 sliced objects for now (lets not worry about explosions).

            //    //Make 2 childPivotPoints for each slicedpart.
            //    //Note: currently the slicedObjects transform position = the parent's transform position of the original piece.
            //    GameObject swordBase = m_swordBase;
            //    for(int i = 0;i < slicedParts.Count; ++i)
            //    {
            //        GameObject slicedPart = slicedParts[i];
            //        GameObject childPivotPoint = Instantiate<GameObject>(childPivotPointPrefab);
            //        childPivotPoint.transform.parent = slicedPart.transform;
            //        //set childPivotPointPosition to the worldspace coordinate for the centerpoint of the sliced sprite.
            //        childPivotPoint.transform.position = slicedPart.GetComponent<SlicedSprite>().MeshRenderer.bounds.center;

            //        //USE THIS INSTEAD as it gets the worldspace coordinates for the centerpoint of the sliced sprite.
            //        //slicedPart.GetComponent<SlicedSprite>().MeshRenderer.bounds.center;
            //        // Debug.Log("!sliced part center point" + i + " : " + slicedPart.GetComponent<SlicedSprite>().MeshRenderer.bounds.center);

            //        //this returns the centerpoint relative to its local position.
            //        //slicedPart.GetComponent<SlicedSprite>().SpriteBounds.center;
            //        //Debug.Log("sliced part center point" + i + " : "  + slicedPart.GetComponent<SlicedSprite>().SpriteBounds.center);
            //        //Debug.Log(childPivotPoint.transform.localPosition);
            //    }


            //    //find the closest branch to base object. ~ all child objects are relative to the parent's location.
            //    Vector3 basePosition = swordBase.transform.position;
            //    GameObject closestObjectToBase;
          
            //    float d1 = Vector3.Distance(slicedParts[0].transform.GetChild(0).transform.position, basePosition);
            //    float d2 = Vector3.Distance(slicedParts[1].transform.GetChild(0).transform.position, basePosition);

            //    Debug.Log(slicedParts[0].name + " " + d1);
            //    Debug.Log(slicedParts[1].name + " " + d2);

            //    if (d1 > d2)
            //    {
            //        closestObjectToBase = slicedParts[1]; 
            //    }
            //    else//d1 <= d2
            //    {
            //        closestObjectToBase = slicedParts[0];
            //    }

             //   Debug.Log("Closest Object from base: " + closestObjectToBase.name);


                //Case 2: sliced object is the base of the sword.
           // }
           // else
          //  {
           //     Debug.Log(" slicedObjectInfo is null");
          //  }
        }

    }


    public GameObject GetSlicedObjectClosestToBase(List<SpriteSlicer2DSliceInfo> slicedObjectInfo, GameObject baseObject)
    {
        if (slicedObjectInfo == null)
            return null;
        //shouldn't have more than one slicedObjectInfo in list unless explosion function was called.
        if(slicedObjectInfo.Count > 1)
        {
            Debug.Log("GetSlicedObjectClosestToBase::slicedObjectInfo count " + slicedObjectInfo.Count);
            foreach(SpriteSlicer2DSliceInfo info in slicedObjectInfo)
            {
                Debug.Log(info.SlicedObject.name);
            }
            return null;
        }

        SpriteSlicer2DSliceInfo branch = slicedObjectInfo[0];
        List<GameObject> slicedParts = branch.ChildObjects;

        //gets the worldspace coordinates for the centerpoint of the sliced sprite.
        Vector3 slicedPartCenter1 = slicedParts[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center;
        Vector3 slicedPartCenter2 = slicedParts[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center;

        float d1 = Vector3.Distance(baseObject.transform.position, slicedPartCenter1);
        float d2 = Vector3.Distance(baseObject.transform.position, slicedPartCenter2);

        GameObject closestObjectToBase = d1 < d2 ? slicedParts[0] : slicedParts[1];

        return closestObjectToBase;
    }

}
