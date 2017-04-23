using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionScript : MonoBehaviour {
    public float overlapRadius;

    private bool smithing;

    PlayerInputManager inputs;
    CharacterMovementScript movement;
    EDGELORD.TreeBuilder.TreeRoot root;

	// Use this for initialization
	void Start () {
        inputs = GetComponent<PlayerInputManager>();
        movement = GetComponent<CharacterMovementScript>();
        root = GameObject.Find("TreeRoot").GetComponent<EDGELORD.TreeBuilder.TreeRoot>();
        smithing = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (smithing == false) {
            if (inputs.getActionDown()) {
                //Do Something
                Collider2D collider = Physics2D.OverlapCircle(transform.position, overlapRadius);
                if (collider != null) {
                    EDGELORD.TreeBuilder.TreeBranch branch = collider.transform.GetComponentInParent<EDGELORD.TreeBuilder.TreeBranch>();
                    smithing = true;
                    movement.movementEnabled = false;
                    StartCoroutine(setDirectionAndPower(branch));
                }
            }
        }
	}

    IEnumerator setDirectionAndPower(EDGELORD.TreeBuilder.TreeBranch branch) {
        yield return null;
        float direction = 0;
        float length = 0;
        while (!inputs.getActionDown()) {
            direction += inputs.getMovementDirection().x*0.5f;
            print(direction);
            length += inputs.getMovementDirection().y*0.2f;
            yield return null;
        }
        direction *= Mathf.Deg2Rad;
        direction += Mathf.PI / 2;
        root.CreateBranch(new EDGELORD.TreeBuilder.TreeBranchData(length, length*0.33f, branch.transform.TransformDirection(new Vector2(-Mathf.Cos(direction), Mathf.Sin(direction))), branch, branch.transform.InverseTransformPoint(transform.position)));
        smithing = false;
        movement.movementEnabled = true;
        yield return null;
    }
}
