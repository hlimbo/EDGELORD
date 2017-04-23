using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	public RangedFloat volume;

    [MinMaxRange(-2, 2)]
    public RangedFloat pitch;

    public float minDistance = 50f;

	public override void Play(AudioSource source, MonoBehaviour player = null)
	{
		if (clips.Length == 0) return;
	    if (!source)
	    {
	        Debug.LogWarning("[SimpleAudioEvent] Cannot play on NULL source!");
            return;
	    }
		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
	    source.minDistance = minDistance;
		source.Play();
	}
}