using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionScript : MonoBehaviour {
    PlayerInputManager inputs;

	// Use this for initialization
	void Start () {
        inputs = GetComponent<PlayerInputManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (inputs.getActionDown()) {
            //Do Something
        }
	}
}
