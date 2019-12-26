using UnityEngine;
using System.Collections;

public class EpicBeamAudioPlayer : AudioPlayer
{
    [SerializeField]
    private AudioClip _epicDrop;

    public AudioClip epicDrop { get { return _epicDrop; } }

    protected override void Start()
    {
        base.Start();
    }

    public void PlayEpicDropAudio()
    {
        _audioSource.clip = _epicDrop;
        _audioSource.Play();
    }
}
