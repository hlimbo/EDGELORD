using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPlayerPositionsScript : MonoBehaviour {

    public GameObject swordRootPlayer1;
    public GameObject player1Avatar;
    private BoxCollider2D anvil1Collider;

    public GameObject swordRootPlayer2;
    public GameObject player2Avatar;
    private BoxCollider2D anvil2Collider;

    private float aspectRatio;

    //the child index of the anvil chosen from each playerAvatar's root in the unity editor
    const int ANVIL = 2;

	// Use this for initialization
	void Start ()
    {
        anvil1Collider = player1Avatar.transform.GetChild(ANVIL).GetComponent<BoxCollider2D>();
        anvil2Collider = player2Avatar.transform.GetChild(ANVIL).GetComponent<BoxCollider2D>();
        aspectRatio = Camera.main.aspect;
        CalculateBlackSmithPositions();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(aspectRatio != Camera.main.aspect)
        {
            //reposition both player avatars with their swords when aspect ratio of the screen changes to prevent blacksmiths from being hidden away from the web browser's window
            
            aspectRatio = Camera.main.aspect;
            CalculateBlackSmithPositions();
        }
	}

    private void CalculateBlackSmithPositions()
    {
        Vector2 camPosition = Camera.main.transform.position;
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = (Camera.main.orthographicSize * 2 * Camera.main.aspect) / 2;

        swordRootPlayer1.transform.position = new Vector2((camPosition.x - camHalfWidth) + anvil1Collider.size.x / 2, camPosition.y - camHalfHeight + anvil1Collider.size.y);
        player1Avatar.transform.position = new Vector2((camPosition.x - camHalfWidth) + anvil1Collider.size.x / 2, camPosition.y - camHalfHeight + anvil1Collider.size.y);

        swordRootPlayer2.transform.position = new Vector2((camPosition.x + camHalfWidth) - anvil2Collider.size.x / 2, camPosition.y - camHalfHeight + anvil2Collider.size.y);
        player2Avatar.transform.position = new Vector2((camPosition.x + camHalfWidth) - anvil2Collider.size.x / 2, camPosition.y - camHalfHeight + anvil2Collider.size.y);
    }
}
