using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedPlayerSetup : NetworkBehaviour
{
    public PlayerInputManager playerInputManager;
    void Start ()
    {
        if (!playerInputManager) playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.inputLocked = true;
        //if (isLocalPlayer)
        //{
        //    playerInputManager.inputLocked = false;
        //}
        //else
        //{
        //    playerInputManager.inputLocked = true;
        //}
	}
    [SyncVar]
    public Vector2 MovementDirection;
    [SyncVar]
    public bool GetActionDown;
    void Update()
    {
        if(isLocalPlayer)
        {
            MovementDirection = playerInputManager.getMovementDirection();
            GetActionDown = playerInputManager.getActionDown();
            //playerInputManager.GetAction = playerInputManager.getAction();
            //playerInputManager.GetActionUp = playerInputManager.getActionUp();
        }
        playerInputManager.MovementDirection = MovementDirection;
        playerInputManager.GetActionDown = GetActionDown;
        //playerInputManager.GetAction = playerInputManager.getAction();
        //playerInputManager.GetActionUp = playerInputManager.getActionUp();

    }
	
}
