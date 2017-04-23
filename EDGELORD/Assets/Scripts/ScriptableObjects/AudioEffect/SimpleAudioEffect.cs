using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Effects/Simple")]
public class SimpleAudioEffect : AudioEffect
{
    [System.Serializable]
    public class AudioConfig
    {
        public AudioClip clip;
        [MinMaxRange(-2, 2)]
        public RangedFloat pitchRange;
        public RangedFloat volumeRange;
        public float minRange;

        public static void Configure(AudioSource source, AudioConfig config, bool looping)
        {
            source.clip = config.clip;
            // ...
        }

    }

    public string name;
    public AudioEvent startEvent;
    public AudioEvent loopEvent;
    public AudioEvent endEvent;

    public bool IsPlaying { get { return _isPlaying; } }
    private bool _isPlaying = false;

    // Defined in Play; either generated or by reference.
    private AudioSource _startSource;
    // Generated in Play if it doesn't already exist.
    private AudioSource _loopSource;
    private GameObject _audioGameObject;

    protected virtual void ConfigureAudioGameObject(AudioSource source, MonoBehaviour player)
    {
        _audioGameObject = (source != null) ? source.gameObject : new GameObject(name + " AudioEffect");
        _startSource = (source != null) ? source : _audioGameObject.AddComponent<AudioSource>();
        _startSource.loop = false;
        _loopSource = _audioGameObject.AddComponent<AudioSource>();
        _loopSource.loop = true;

        if (player != null)
        {
            _audioGameObject.transform.SetParent(player.gameObject.transform);
        }
    }

    public override void Play(AudioSource source = null, MonoBehaviour player = null)
    {
        if(_isPlaying) return;
        if (_audioGameObject == null)
        {
            ConfigureAudioGameObject(source, player);
        }
        if (_audioGameObject == null)
        {
            Debug.LogError("Audio GameObject for SimpleAudioEffect failed to generate.");
        }
        //_audioGameObject.SetActive(true);
        startEvent.Play(_startSource, player);
        loopEvent.Play(_loopSource, player);
        _isPlaying = true;
    }

    public override void Loop(AudioSource source = null, MonoBehaviour player = null)
    {
        //throw new System.NotImplementedException();
    }

    public override void Stop(AudioSource source = null, MonoBehaviour player = null)
    {
        endEvent.Play((source != null)? source : _startSource, player);
        _loopSource.Stop();
        _isPlaying = false;
    }
}
