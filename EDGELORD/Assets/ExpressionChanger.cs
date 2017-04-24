using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionChanger : MonoBehaviour {

    private int currentFaceIndex = 0;
    private int faceCooldown = 500;
    public Sprite[] facials;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        faceCooldown--;
        if (faceCooldown <= 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = facials[changeFace()];
            faceCooldown = 500;
        }
	}

    private int changeFace()
    {
        int index = currentFaceIndex;
        while (index == currentFaceIndex)
        {
            index = Random.Range(0, 3);
        }
        return index;
    }
}
