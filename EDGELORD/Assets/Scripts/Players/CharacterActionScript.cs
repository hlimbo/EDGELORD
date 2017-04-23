using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

public class CharacterActionScript : MonoBehaviour {
    public float overlapRadius;
    public PlayerID OwningPlayer;
    [Space]
    public float maxLength;
    public float minLength;

    private bool smithing;

    PlayerInputManager inputs;
    CharacterMovementScript movement;
    EDGELORD.TreeBuilder.TreeRoot root;
    Transform ghostBlade;
    GhostBladeScript bladeScript;

    // Use this for initialization
    void Start() {
        inputs = GetComponent<PlayerInputManager>();
        movement = GetComponent<CharacterMovementScript>();
        foreach (EDGELORD.TreeBuilder.TreeRoot treeRoot in GameObject.FindObjectsOfType<EDGELORD.TreeBuilder.TreeRoot>()) {
            if (OwningPlayer == treeRoot.OwningPlayer) {
                root = treeRoot;
            }
        }
        smithing = false;
        ghostBlade = transform.GetChild(0);
        ghostBlade.gameObject.SetActive(false);
        bladeScript = ghostBlade.GetComponent<GhostBladeScript>();
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
        ghostBlade.gameObject.SetActive(true);
        float direction = 0;
        float length = minLength;
        while (!inputs.getActionDown()) {
            direction += inputs.getMovementDirection().x*0.5f;
            float radDirection = (direction * Mathf.Deg2Rad) + (Mathf.PI / 2);
            length = Mathf.Clamp(length+inputs.getMovementDirection().y*0.1f, minLength, maxLength);
            bladeScript.setRotation(branch.transform.TransformDirection(new Vector2(-Mathf.Cos(radDirection), Mathf.Sin(radDirection))));
            bladeScript.setScale(new Vector2(2.0f/length, length));
            yield return null;
        }
        direction *= Mathf.Deg2Rad;
        direction += Mathf.PI / 2;
        root.CreateBranch(new EDGELORD.TreeBuilder.TreeBranchData(length, 2.0f/length, branch.transform.TransformDirection(new Vector2(-Mathf.Cos(direction), Mathf.Sin(direction))), branch, branch.transform.InverseTransformPoint(transform.position)));
        smithing = false;
        movement.movementEnabled = true;
        bladeScript.setRotation(new Vector2());
        bladeScript.setScale(new Vector2(1,1));
        ghostBlade.gameObject.SetActive(false);
        yield return null;
    }
}
