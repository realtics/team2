using UnityEngine;
using System.Collections;

public enum MonsterAudioType
{
	Attack,
	Die,
	Hit,
	Skill,
}

public class MonsterAudioPlayer : AudioPlayer
{
	[SerializeField]
	private AudioClip _attack;
	[SerializeField]
	private AudioClip _die;
	[SerializeField]
	private AudioClip _hit;
	[SerializeField]
	private AudioClip _skill;

	protected override void Start()
	{
		base.Start();
	}

	public void PlayMonsterSound(MonsterAudioType monsterAudioType)
	{
		if (monsterAudioType == MonsterAudioType.Attack)
			PlayAudio(_attack);
		else if (monsterAudioType == MonsterAudioType.Die)
			PlayAudio(_die);
		else if (monsterAudioType == MonsterAudioType.Hit)
			PlayAudio(_hit);
		else if (monsterAudioType == MonsterAudioType.Skill)
			PlayAudio(_skill);
	}
}
