using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;
using EDGELORD.Manager;

public class CharacterActionScript : MonoBehaviour {
    public float overlapRadius;
    public PlayerID OwningPlayer;
    [Space]
    public float maxLength;
    public float minLength;
    public float rotationSpeed;
    public float lengthChangeSpeed;
    public float minWidth;
    public float maxWidth;


    private Vector3 _startPos;
    private bool smithing;
    private bool ready;

    public bool isReady { get { return ready; } }

    GameManager manager;
    PlayerInputManager inputs;
    CharacterMovementScript movement;
    EDGELORD.TreeBuilder.TreeRoot root;
    Transform ghostBlade;
    GhostBladeScript bladeScript;
    SfxPlayer sfxPlayer;
    CameraShakeScript screenShake;

    // Use this for initialization
    void Start()
    {
        _startPos = transform.position;
        manager = GameObject.FindObjectOfType<GameManager>();
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
        sfxPlayer = GetComponent<SfxPlayer>();
        screenShake = Camera.main.GetComponent<CameraShakeScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!smithing) {
            if (inputs.getActionDown()) {
                Collider2D collider = Physics2D.OverlapCircle(transform.position, overlapRadius, LayerMask.GetMask("Default"));
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, LayerMask.GetMask("Default"));
                foreach (Collider2D c in colliders)
                {
                    var branch = c.transform.GetComponentInParent<EDGELORD.TreeBuilder.TreeBranch>();
                    if (branch)
                    {
                        if (branch.GetProjectedPosition(transform.position).magnitude <= branch.BranchLength)
                        {
                            if (branch.OwningPlayer == OwningPlayer)
                            {
                                collider = c;
                                break;
                            }
                        }
                    }
                }
                if (collider != null) {
                    sfxPlayer.PlaySoundEffect("sword_hit");
                    EDGELORD.TreeBuilder.TreeBranch branch = collider.transform.GetComponentInParent<EDGELORD.TreeBuilder.TreeBranch>();
                    if (branch)
                    {
                        if (branch.GetProjectedPosition(transform.position).magnitude <= branch.BranchLength)
                        {
                            if (branch.OwningPlayer == OwningPlayer)
                            {
                                ready = true;
                                smithing = true;
                                movement.movementEnabled = false;
                                transform.position = branch.GetProjectedPosition(transform.position, true);
                                StartCoroutine(setDirectionAndPower(branch));
                            }
                        }
                    }
                    else
                    {
                        print(collider.gameObject.name);
                        Debug.LogWarning("No Branch Found!!!");
                    }
                }
            }
        }
	}

    IEnumerator setDirectionAndPower(EDGELORD.TreeBuilder.TreeBranch branch) {
        yield return null;
        ghostBlade.gameObject.SetActive(true);
        float direction = 0;
        float length = minLength;
        while ((!inputs.getActionDown() && branch.IsAttached && branch.IsPointOnBranch(transform.position))//branch.GetProjectedPosition(transform.position).magnitude<=branch.BranchLength) 
            || !manager.gameRunning) {
            direction += inputs.getMovementDirection().x*rotationSpeed*Time.deltaTime;
            float radDirection = (direction * Mathf.Deg2Rad) + (Mathf.PI / 2);
            length = Mathf.Clamp(length+inputs.getMovementDirection().y*lengthChangeSpeed*Time.deltaTime, minLength, maxLength);
            bladeScript.setRotation(branch.transform.TransformDirection(new Vector2(-Mathf.Cos(radDirection), Mathf.Sin(radDirection))));
            bladeScript.setScale(new Vector2(Mathf.Clamp(2.0f / length, minWidth, maxWidth), length));
            yield return null;
        }
        if (branch.IsAttached && branch.IsPointOnBranch(transform.position))//branch.GetProjectedPosition(transform.position).magnitude <= branch.BranchLength)
        {
            direction *= Mathf.Deg2Rad;
            direction += Mathf.PI / 2;
            var resultBranch = root.CreateBranch(new EDGELORD.TreeBuilder.TreeBranchData(length, Mathf.Clamp(2.0f / length, minWidth, maxWidth), branch.transform.TransformDirection(new Vector2(-Mathf.Cos(direction), Mathf.Sin(direction))), branch, branch.transform.InverseTransformPoint(transform.position)));
            //Vector3 endpoint = resultBranch.transform.position + resultBranch.transform.up * (resultBranch.BranchLength-0.1f);
            //movement.moveToPoint(endpoint);
            sfxPlayer.PlaySoundEffect("sword_thrust");
        }
        else {
            //branch was broken
            ResetPosition();
            screenShake.screenShake(0.2f, 0.1f);
        }
        smithing = false;
        movement.movementEnabled = true;
        bladeScript.setRotation(new Vector2());
        bladeScript.setScale(new Vector2(1,1));
        ghostBlade.gameObject.SetActive(false);
        yield return null;
    }

    public void setNotReady(){
        ready = false;
    }

    public void ResetPosition()
    {
        transform.position = _startPos;
    }
}
