using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    public bool inputsEnabled = true;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode action;
	
    public Vector2 getMovementDirection() {
        if (inputsEnabled) {
            int vertical = 0;
            int horizontal = 0;
            if (Input.GetKey(up)) {
                vertical += 1;
            }
            if (Input.GetKey(down)) {
                vertical -= 1;
            }
            if (Input.GetKey(right)) {
                horizontal += 1;
            }
            if (Input.GetKey(left)) {
                horizontal -= 1;
            }
            return new Vector2(horizontal, vertical);
        }
        else {
            return new Vector2(0,0);
        }
    }

    public bool getActionDown() {
        return Input.GetKeyDown(action) && inputsEnabled;
    }

    public bool getAction() {
        return Input.GetKey(action) && inputsEnabled;
    }
}
