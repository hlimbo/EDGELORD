using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script illustrates where an object is sliced using Debug.DrawLine
public class CutProto : MonoBehaviour {

    public GameObject Watermelon;
    public float sliceDistance = 500.0f;

    [SerializeField]
    List<SpriteSlicer2DSliceInfo> slicedObjectInfo = null;

    public bool canDeleteParent = false;
  
    void Start()
    {
        Vector3 endPosition = transform.position + (Vector3.right * sliceDistance);
        //SpriteSlicer2D.SliceAllSprites(Watermelon.transform.position, endPosition);
        //Debug.DrawLine(transform.position, endPosition, Color.red, 120.0f);
        SpriteSlicer2D.SliceSprite(transform.position, endPosition, Watermelon,canDeleteParent,ref slicedObjectInfo);
        //SpriteSlicer2D.ExplodeSprite(Watermelon, 10, 10.0f);
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(transform.position, mousePos, Color.red);
        }
    }
  
}
