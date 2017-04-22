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


        
        if (Input.GetMouseButton(0))
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(startPoint, endPoint, Color.red);
        }

        if(Input.GetMouseButtonUp(0))
        {
            
            SpriteSlicer2D.SliceAllSprites(startPoint, endPoint, canDestroyParent, ref slicedObjectInfo, slicableMask);

            if (slicedObjectInfo != null)
            {
                //retrieve the 2nd child and apply gravity to it.
                Debug.Log("List Count: " + slicedObjectInfo.Count);

                slicedObjectInfo[0].ChildObjects[1].GetComponent<Rigidbody2D>().gravityScale = 5;
                slicedObjectInfo[0].ChildObjects[0].GetComponent<Rigidbody2D>().isKinematic = true;

                //Case 1: sliced object is the branch of the sword.
                SpriteSlicer2DSliceInfo branch = slicedObjectInfo[0];
                List<GameObject> slicedParts = branch.ChildObjects;//should only return 2 sliced objects for now (lets not worry about explosions).

                //Make 2 childPivotPoints for each slicedpart.
                //Note: currently the slicedObjects transform position = the parent's transform position of the original piece.
                GameObject swordBase = m_swordBase;
                int i = 0;
                foreach (GameObject slicedPart in slicedParts)
                {
                    GameObject childPivotPoint = Instantiate<GameObject>(childPivotPointPrefab);
                    childPivotPoint.transform.parent = slicedPart.transform.transform;
                    //set childPivotPointPosition relative to slicedPart's transform.position
                    childPivotPoint.transform.localPosition = Vector3.zero;

                    //set the childPivotPointPosition relative to the topside of the slicedObject.
                    //Debug.Log(slicedPart.GetComponent<SlicedSprite>().SpriteBounds.size);

                    Vector3 offset = slicedPart.GetComponent<SlicedSprite>().SpriteBounds.size / 2;

                    if (i == 1)
                        childPivotPoint.transform.localPosition -= new Vector3(0, offset.y);

                    Debug.Log(childPivotPoint.transform.localPosition);
                    i++;
                }


                //find the closest branch to base object. ~ all child objects are relative to the parent's location.
                Vector3 basePosition = swordBase.transform.position;
                GameObject closestObjectToBase = slicedParts[0];
                float minDistanceFromBase = (slicedParts[0].transform.GetChild(0).transform.position - basePosition).magnitude;
                float distanceFromBase = (slicedParts[1].transform.GetChild(0).transform.position - basePosition).magnitude;

                Debug.Log(closestObjectToBase.name + " " + minDistanceFromBase);
                Debug.Log(slicedParts[1].name + " " + distanceFromBase);

                if (distanceFromBase < minDistanceFromBase)
                {
                    closestObjectToBase = slicedParts[1]; 
                }

                Debug.Log("Closest Object from base: " + closestObjectToBase.name);


                //Case 2: sliced object is the base of the sword.
            }
            else
            {
                Debug.Log(" slicedObjectInfo is null");
            }
        }

    }

}
