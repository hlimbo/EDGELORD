using UnityEngine;
using System.Collections;

/* 
HOW TO USE:
Drag the MusicPlayer into your scene, then drag a MusicTrack prefab into the scene
as a child of MusicPlayer.

MusicPlayer options:
-Volume: the volume to play the music at.
-Start Delay: after MusicPlayer has StartMusiced, how many seconds to wait before loading the first AudioClip.
-Start On Init: when checked, MusicPlayer calls its StartMusic() function immediately upon creation. 
	when unchecked, MusicPlayer does not StartMusic until the StartMusic() function is explicitly called.
*/

public class MusicPlayer : Singleton<MusicPlayer> {

	public float Volume = 1.0F;
	public double StartMusicDelay = 0.0F;
	public bool StartMusicOnInit = true;

	private MusicTrack track;
	private AudioSource[] audioSources;
	private float currentVolume = 1.0F;
	private double nextEventTime;
	private int flip = 0;
	private bool isPlaying = false;

    protected MusicPlayer() {}

	// Use this for initialization
	void Start () {
		audioSources = new AudioSource[2];
		for (int i = 0; i < audioSources.Length; ++i) {
			GameObject child = new GameObject("Audio Source");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].volume = Volume;
		}

		track = GetComponentInChildren<MusicTrack>();

		if (StartMusicOnInit) {
			StartMusic();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPlaying) {
			return;
		}
		double time = AudioSettings.dspTime;

		if (time + 1.0F > nextEventTime) {
			MusicClip musicClip = track.GetNextClip();
			audioSources[flip].clip = musicClip.Clip;
			audioSources[flip].PlayScheduled(nextEventTime);
			nextEventTime += musicClip.LengthInSeconds;
			flip = 1 - flip;
		}
	}

	public void StartMusic() {
		isPlaying = true;
		nextEventTime = AudioSettings.dspTime + StartMusicDelay;
		Debug.Log("MusicPlayer started");
	}

	public void StopMusic() {
		isPlaying = false;
		FadeOutAndStop(0.06f); // fadeout super quick so that the audio doesn't pop when stopped
		Debug.Log("MusicPlayer stopped");
	}

	public void ResetVolume() {
		foreach (var source in audioSources) {
			source.volume = Volume;
		}
	}

	public void FadeOut(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		StartCoroutine(fadeOutCoroutine(lengthInSeconds, interval));
	}

	public void FadeOutAndStop(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		StartCoroutine(fadeOutAndStopCoroutine(lengthInSeconds, interval));
	}

	private IEnumerator fadeOutCoroutine(float lengthInSeconds, float interval) {
		while (currentVolume > 0.0F) {
			foreach (var source in audioSources) {
				source.volume -= interval;
			}
			currentVolume -= interval;		
			yield return new WaitForSeconds(lengthInSeconds * interval);
		}
	}

	private IEnumerator fadeOutAndStopCoroutine(float lengthInSeconds, float interval) {
		while (currentVolume > 0.0F) {
			foreach (var source in audioSources) {
				source.volume -= interval;
			}
			currentVolume -= interval;		
			yield return new WaitForSeconds(lengthInSeconds * interval);
		}
		foreach (var source in audioSources) {
			source.Stop();
		}
		ResetVolume();
	}
}

