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
    void Start()
    {
        SpriteSlicer2D.DebugLoggingEnabled = true;
        startPoint = transform.position;
    }

    void Update()
    {
        //A crude function to slice objects 
        startPoint = transform.position;
        if (Input.GetMouseButton(0))
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(startPoint, endPoint, Color.red);
        }

        if (Input.GetMouseButtonUp(0))
        {

            SpriteSlicer2D.SliceAllSprites(startPoint, endPoint, canDestroyParent, ref slicedObjectInfo, slicableMask);

            //Case 2 reparent branches to proper slices if main part of the sword is cut in half
            //DONE: Reparent the children branches attached to the base of the sliced object.
            SpriteSlicer2DSliceInfo info = GetMostRecentSlicedObject(slicedObjectInfo);

            //ReparentBranchesToSlicedObjects(SpriteSlicer2DSliceInfo recentInfo)
            GameObject slicedObject = info.SlicedObject;
            List<GameObject> slicedPieces = info.ChildObjects;

            //obtain all the branches from the slicedObject.
            List<GameObject> branches = new List<GameObject>();
            for (int i = 0; i < slicedObject.transform.childCount; ++i)
                branches.Add(slicedObject.transform.GetChild(i).gameObject);

            //detach branches from deactivated parent object
            slicedObject.transform.DetachChildren();

            foreach(GameObject branch in branches)
            {
                //obtain which branch is closer of the 2 slicedPiece objects
                float d1 = Vector3.Distance(slicedPieces[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center, branch.transform.position);
                float d2 = Vector3.Distance(slicedPieces[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center, branch.transform.position);

                //float d1 = Vector3.Distance(branch.transform.position, slicedPieces[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center);
                //float d2 = Vector3.Distance(branch.transform.position, slicedPieces[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center);

                //if slicedPieces[0] is further away from slicedPieces[1] -> attach branch to slicedPieces[1]
                if (d1 > d2)
                    branch.transform.SetParent(slicedPieces[1].transform, true);
                else
                    branch.transform.SetParent(slicedPieces[0].transform, true);
            }

            //Test pls!

            //calculate 

            // SAMPLE USAGE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //variables to pass into this function
            //SpriteSlicer2DSliceInfo info = GetMostRecentSlicedObject(slicedObjectInfo);
            //GameObject slicedObject = GetSlicedObjectClosestToBase(info, m_swordBase);

            //if(slicedObject != null)
            //{
            //    Debug.Log(slicedObject.name);
            //}

        }

    }


    public SpriteSlicer2DSliceInfo GetMostRecentSlicedObject(List<SpriteSlicer2DSliceInfo> slicedObjectInfo)
    {
        if (slicedObjectInfo == null)
        {
            Debug.Log("GetMostRecentSlicedObject slicedObjectInfo is null");
            return null;
        }

        if (slicedObjectInfo.Count == 1)
        {
            return slicedObjectInfo[0];
        }

        //GET THE last object in the array is the most recently sliced.
        int recentlySlicedIndex = slicedObjectInfo.Count - 1;
        return slicedObjectInfo[recentlySlicedIndex];
    }
    public GameObject GetSlicedObjectClosestToBase(SpriteSlicer2DSliceInfo branch, GameObject baseObject)
    {
        if (branch == null)
            return null;

        //Note: there should be only 2 sliced parts in the list
        List<GameObject> slicedParts = branch.ChildObjects;

        //gets the worldspace coordinates for the centerpoint of the sliced sprite.
        Vector3 slicedPartCenter1 = slicedParts[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center;
        Vector3 slicedPartCenter2 = slicedParts[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center;

        float d1 = Vector3.Distance(baseObject.transform.position, slicedPartCenter1);
        float d2 = Vector3.Distance(baseObject.transform.position, slicedPartCenter2);

        GameObject closestObjectToBase = d1 < d2 ? slicedParts[0] : slicedParts[1];

        return closestObjectToBase;
    }

    ////used to obtain which branches need to be reattached to the baseObject
    //public bool IsBranchClosestToBase(GameObject branch, GameObject baseObject)
    //{
    //    Vector3 branchCenter = branch.GetComponent<SpriteRenderer>().bounds.center;

    //    float d1 = Vector3.Distance(baseObject.transform.position, branch.transform.position);
    //    if()

    //    return null;
    //}

    //TODO: Get how much area you have based on the sword's area
    //include: area of each metal segment attached to the base of the sword.
    //include: base of the sword.
    //hard: area between the branches of the sword.
}

/* Garbo Logic for determining the distance from base to branch
 * 
 *  //if (slicedObjectInfo != null)
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
 */

