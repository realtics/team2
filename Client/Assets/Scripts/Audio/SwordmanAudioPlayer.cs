using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanAudioPlayer : AudioPlayer
{
	public List<AudioClip> attackClip;
	public List<AudioClip> jumpAttackClip;

	public AudioClip jingongClip;
	public AudioClip hadoukenClip;
	public AudioClip blacheClip;

	public AudioClip jingongHitClip;
	public AudioClip hadoukenHitClip;
	public AudioClip blacheHitClip;

	protected override void Start()
    {
		base.Start();
    }

	public void PlayerAttackSound(int idx)
	{
		PlayAudio(attackClip[idx]);
	}
}
