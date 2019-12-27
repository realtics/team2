using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanAudioPlayer : AudioPlayer
{
	public List<AudioClip> attackClip;
	public List<AudioClip> jumpAttackClip;

	public AudioClip blacheClip;
	public AudioClip jingongClip;
	public AudioClip onDamageClip;
	public AudioClip jumpClip;

	protected override void Start()
    {
		base.Start();
    }

	public void PlayAttackSound(int idx)
	{
		PlayAudio(attackClip[idx]);
	}

	public void PlayJumpAttackSound()
	{
		int rand = Random.Range(0, 2);

		if (rand == 0)
			PlayAudio(jumpAttackClip[0]);
		else
			PlayAudio(jumpAttackClip[1]);
	}

	public void PlayBlacheSpawnClip()
	{
		PlayAudio(blacheClip);
	}

	public void PlayJingongSpawnClip()
	{
		PlayAudio(jingongClip);
	}

	public void PlayJumpClip()
	{
		PlayAudio(jumpClip);
	}

	public void PlayHitClip()
	{
		PlayAudio(onDamageClip);
	}
}
