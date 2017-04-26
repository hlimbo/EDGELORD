using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementScript : MonoBehaviour {
    public float moveSpeed;
    public float acceleration;
    public bool movementEnabled = true;

    PlayerInputManager inputs;
    Rigidbody2D rigid;

    // Use this for initialization
    void Start () {
        inputs = GetComponent<PlayerInputManager>();
        rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (movementEnabled) {
            Vector2 direction = Vector2.ClampMagnitude(Vector2.MoveTowards(rigid.velocity, moveSpeed * inputs.MovementDirection, moveSpeed * acceleration * Time.deltaTime), moveSpeed);
            rigid.velocity = direction;
        }
        else {
            rigid.velocity = new Vector2();
        }
	}

    public void moveToPoint(Vector2 point) {
        transform.position = point;
    }
}
