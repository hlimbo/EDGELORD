using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Unlike AudioEvents, AudioEffects are continuous and have specific start, looping, and stopping sounds.
/// </summary>
public abstract class AudioEffect : ScriptableObject
{
    //public abstract void Play(MonoBehaviour player = null);
    public abstract void Play(AudioSource source = null, MonoBehaviour player = null);
    public abstract void Loop(AudioSource source = null, MonoBehaviour player = null);
    public abstract void Stop(AudioSource source = null, MonoBehaviour player = null);
}
