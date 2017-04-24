using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySignWoosh : MonoBehaviour {

    public AudioClip wood;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playWoosh()
    {
        audioSource.pitch += (Random.value - 0.5f) * 0.5f;
        audioSource.Play();
    }
}
