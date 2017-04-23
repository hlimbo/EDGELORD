using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode action;
	
    public Vector2 getMovementDirection() {
        int vertical = 0;
        int horizontal = 0;
        if (Input.GetKey(up)) {
            vertical += 1;
        }
        if (Input.GetKey(down)){
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

    public bool getActionDown() {
        return Input.GetKeyDown(action);
    }

    public bool getAction() {
        return Input.GetKey(action);
    }
}
