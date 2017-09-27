using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uses the 4 collider game objects and attaches them to the 4 edges of the camera's bounds
public class BoundaryScript : MonoBehaviour {

    public enum SIDES { LEFT = 0, RIGHT, TOP, BOTTOM };

    public GameObject[] walls;
    private BoxCollider2D[] wc;
    private Resolution res;
    private float aspectRatio;

	// Use this for initialization
	void Start ()
    {
        wc = new BoxCollider2D[4];
        for(int i = 0;i < walls.Length; ++i)
        {
            wc[i] = walls[i].GetComponent<BoxCollider2D>();
        }
        aspectRatio = Camera.main.aspect;
        res = Screen.currentResolution;
        CalculateBoundaryPositions();
        
	}

    private void Update()
    {
        if(aspectRatio != Camera.main.aspect)
        {
            CalculateBoundaryPositions();
            res = Screen.currentResolution;
            aspectRatio = Camera.main.aspect;
            Debug.Log("re calculated boundary positions");
        }
    }

    private void CalculateBoundaryPositions()
    {
        Vector2 camPosition = Camera.main.transform.position;
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = (Camera.main.aspect * camHalfHeight * 2) / 2;
        walls[(int)SIDES.LEFT].transform.position = new Vector2(camPosition.x - camHalfWidth - wc[(int)SIDES.LEFT].bounds.extents.x, camPosition.y);
        walls[(int)SIDES.RIGHT].transform.position = new Vector2(camPosition.x + camHalfWidth + wc[(int)SIDES.RIGHT].bounds.extents.x, camPosition.y);
        walls[(int)SIDES.TOP].transform.position = new Vector2(camPosition.x, camPosition.y + camHalfHeight + wc[(int)SIDES.TOP].bounds.extents.y);
        walls[(int)SIDES.BOTTOM].transform.position = new Vector2(camPosition.x, camPosition.y - camHalfHeight - wc[(int)SIDES.TOP].bounds.extents.y);
    }

}
