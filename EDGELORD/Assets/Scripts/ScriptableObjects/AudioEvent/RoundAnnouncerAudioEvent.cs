using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Audio Events/RoundAnnouncer")]
public class RoundAnnouncerAudioEvent : AudioEvent
{
    public AudioClip roundClip;
    public AudioClip[] numbersClips;

    public RangedFloat volume;

    [MinMaxRange(-2, 2)]
    public RangedFloat pitch;

    private int roundNumber = 0;

    public override void Play(AudioSource source, MonoBehaviour player = null)
    {
        if((roundNumber + 1) > numbersClips.Length - 1)
        {
            Debug.Log("Not enough clips to play this.");
        }
        player.StartCoroutine(CoConcatenateClips(source, ++roundNumber));
        //source.clip = roundClip;
        //source.loop = false;
        //source.Play();
        //source.PlayOneShot(numbersClips[++roundNumber]);
    }

    private IEnumerator CoConcatenateClips(AudioSource source, int clipIndex)
    {
        source.clip = roundClip;
        source.loop = false;
        source.Play();
        while(source.isPlaying)
        {
            yield return null;
        }
        source.PlayOneShot(numbersClips[clipIndex]);
    }


}
