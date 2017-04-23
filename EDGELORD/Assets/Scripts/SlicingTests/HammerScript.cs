using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Get hammer to swing with input.
public class HammerScript : MonoBehaviour
{
    public Players.PlayerID playerID;

    private Animator animator;
    private SfxPlayer sfxPlayer;
    private PlayerInputManager inputs;

    public Animation animation;

    void  Start()
    {
        animator = GetComponent<Animator>();
        sfxPlayer = FindObjectOfType<SfxPlayer>();

        // get reference to the player's PlayerInputManager to read inputs
        string playerToFind = null;
        if (playerID == Players.PlayerID.Player_1)
        {
            playerToFind = "Player1";
        }
        else if (playerID == Players.PlayerID.Player_2)
        {
            playerToFind = "Player2";
        }

        if(playerToFind != null)
        {
            inputs = (PlayerInputManager)GameObject.Find(playerToFind).GetComponent<PlayerInputManager>();
        }

        animator.StopPlayback();
    }

    void Update()
    {
        if(inputs.getActionDown())
        {
            animator.SetBool("canSwingHammer", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name.Equals("Anvil"))
        {
            Debug.Log("anvil");
            animator.SetBool("canSwingHammer", false);
        }
    }


}
