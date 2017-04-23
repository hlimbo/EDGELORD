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

    //public GameObject particleFXPrefab;
    public GameObject particleFXGO;
    private ParticleSystem[] particles;

    //hacky
    private bool gameStart = true;

    void  Start()
    {
        animator = GetComponent<Animator>();
        sfxPlayer = GetComponent<SfxPlayer>();
        particles = particleFXGO.GetComponentsInChildren<ParticleSystem>();

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

        if (inputs == null)
            Debug.Log("hello");
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
        if(gameStart && other.name.Equals("Anvil"))
        {
            gameStart = false;
        }
        else if(other.name.Equals("Anvil"))
        {
            animator.SetBool("canSwingHammer", false);
            int r = Random.Range(0, sfxPlayer.soundEffects.Length);
            if (r == 0)
                sfxPlayer.PlaySoundEffect("anvil_hit_1");
            else
                sfxPlayer.PlaySoundEffect("anvil_hit_2");

            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
        }
    }


}
