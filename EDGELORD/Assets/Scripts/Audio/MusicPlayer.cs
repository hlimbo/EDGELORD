using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
	public bool StartMusicOnInit = false;

	public GameObject[] tracks;
    private MusicTrack currentTrack;
	private AudioSource[] audioSources;
	private float currentVolume = 1.0F;
	private double nextEventTime;
	private int flip = 0;
	private bool isPlaying = false;
    private bool isInitialized = false;

    private Dictionary<string, MusicTrack> trackDict;

    protected MusicPlayer() {}

    public override void Awake()
    {
        base.Awake();
        if (MusicPlayer.Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        if (!isInitialized)
        {
            Init();
        }
        //DontDestroyOnLoad(this.gameObject);
        //SetChildrenDontDestroyOnLoad(transform);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            //Start();
            
            switch (scene.buildIndex)
            {
                case 0:
                    ForcePlayTrack("Menu Music", true);
                    //StartMusic();
                    //PlayMusic("Menu Music");
                    break;
                case 1:
                    ForcePlayTrack("Ingame Music", false);
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                default:
                    StopMusic();
                    ResetVolume();
                    currentTrack.ResetClips();
                    break;
            }

            //Debug.Log("NextEventTime = " + nextEventTime);
            //isPlaying = false;
        };
    }

    void SetChildrenDontDestroyOnLoad(Transform t)
    {
        foreach (Transform child in t)
        {
            DontDestroyOnLoad(child.gameObject);
            if (child.childCount > 0)
                SetChildrenDontDestroyOnLoad(child);
        }
    }
	// Use this for initialization
	void Start ()
    {
        //if (!isInitialized)
        //{
        //    Init();
        //}
        //StopMusic();
        //ResetVolume();
        //if(StartMusicOnInit)
        //    StartMusic();
        //nextEventTime = 0f;
        //StartMusic();
        //Debug.Log("NextEventTime = " + nextEventTime);
        //isPlaying = false;
    }

    private void Init()
    {
        audioSources = new AudioSource[2];
        trackDict = new Dictionary<string, MusicTrack>();
        for (int i = 0; i < audioSources.Length; ++i)
        {
            GameObject child = new GameObject("Audio Source");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].volume = Volume;
        }

        // instantiate prefab as an object, then add to trackDict to refer to it by name
        foreach (GameObject trackPrefab in tracks)
        {
            var trackObject = Instantiate(trackPrefab, gameObject.transform);
            trackObject.transform.parent = gameObject.transform;
            Debug.Log("added " + trackPrefab.name + " to tracks.");
            trackDict[trackPrefab.name] = trackObject.GetComponent<MusicTrack>();
        }

        currentTrack = trackDict[tracks[0].name]; // default is first child MusicTrack in the editor

        // if (StartMusicOnInit)
        // {
        //     StartMusic();
        // }

        isInitialized = true;
    }

    // Update is called once per frame
    void Update () {
		if (!isPlaying) {
			return;
		}
		double time = AudioSettings.dspTime;

		if (time + 1.0F > nextEventTime) {
			MusicClip musicClip = currentTrack.GetNextClip();
			audioSources[flip].clip = musicClip.Clip;
			audioSources[flip].PlayScheduled(nextEventTime);
			nextEventTime += musicClip.LengthInSeconds;
			flip = 1 - flip;
            Debug.Log("Next Thing");
		}
	}

	public void StartMusic() {
        if (isPlaying) return;

		nextEventTime = 1 + AudioSettings.dspTime + StartMusicDelay;
		isPlaying = true;
		Debug.Log("MusicPlayer started");
	}

    public void PlayMusic(string trackName) {
        if (isPlaying) {
            isPlaying = false;
            StartCoroutine(stopAndSwitchTrackCoroutine(trackName));
        } else {
            currentTrack = trackDict[trackName];
            StartMusic();
        }
    }

    public void ForcePlayTrack(string trackName, bool autoStart)
    {
        StopMusic();
        ResetVolume();
        currentTrack = trackDict[trackName];
        currentTrack.ResetClips();
        if(autoStart) StartMusic();
    }

    private IEnumerator stopAndSwitchTrackCoroutine(string trackName) {
        yield return fadeOutAndStopCoroutine(0.001F, 0.55F);
        ResetVolume();
        currentTrack = trackDict[trackName];
        StartMusic();
    }

	public void StopMusic() {
        if(!isPlaying) return;
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

