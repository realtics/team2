using UnityEngine;
using System.Collections;

public class EpicBeamAudioPlayer : AudioPlayer
{
    [SerializeField]
    private AudioClip _epicDrop;

    protected override void Start()
    {
        base.Start();
	}

	public void PlayEpicAppearSound()
	{
		PlayAudio(_epicDrop);
	}
}
