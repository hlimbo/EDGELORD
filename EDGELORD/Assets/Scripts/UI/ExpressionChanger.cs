using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionChanger : MonoBehaviour {

    private int currentFaceIndex = 0;
    private int faceCooldown = 500;
    public Sprite[] facials;

    //used if placed in gameworld
    private SpriteRenderer sr;
    //used if placed in ui
    private Image img;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        faceCooldown--;
        if (faceCooldown <= 0)
        {
            if (sr)
                sr.sprite = facials[changeFace()];
            else
                img.sprite = facials[changeFace()];

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
