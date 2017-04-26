using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    public bool inputLocked = false;
    public bool inputsEnabled = true;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode action;

    public Vector2 MovementDirection { get; set; }
    public bool GetAction { get; set; }
    public bool GetActionDown { get; set; }
    public bool GetActionUp { get; set; }

    void Update()
    {
        if (inputLocked) return;
        MovementDirection = getMovementDirection();
        GetAction = getAction();
        GetActionDown = getActionDown();
        GetActionUp = getActionUp();
    }


    public Vector2 getMovementDirection()
    {
        if (inputsEnabled)// && !inputLocked)
        {
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
        return Input.GetKeyDown(action) && inputsEnabled;// && !inputLocked;
    }

    public bool getAction() {
        return Input.GetKey(action) && inputsEnabled;// && !inputLocked;
    }

    public bool getActionUp() {
        return Input.GetKeyUp(action) && inputsEnabled;// && !inputLocked;
    }
}
