using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    protected AudioSource _audioSource;

    // Use this for initialization
    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

	protected void PlayAudio(AudioClip clip)
	{
		_audioSource.clip = clip;
		_audioSource.Play();
	}

	public void SetClipNull()
	{
		_audioSource.clip = null;
	}
}
