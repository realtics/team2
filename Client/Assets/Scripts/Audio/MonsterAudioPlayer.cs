using UnityEngine;
using System.Collections;

public class MonsterAudioPlayer : AudioPlayer
{
    [SerializeField]
    private AudioClip _attack;
    [SerializeField]
    private AudioClip _die;
    [SerializeField]
    private AudioClip _damage;
    [SerializeField]
    private AudioClip _skill;

    public AudioClip attack { get { return _attack; } }
    public AudioClip die { get { return _die; } }
    public AudioClip damage { get { return _damage; } }
    public AudioClip skill { get { return _skill; } }

    protected override void Start()
    {
        base.Start();
    }

    public void PlayAttackAudio()
    {
        _audioSource.clip = _attack;
        _audioSource.Play();
    }
    public void PlayDieAudio()
    {
        _audioSource.clip = _die;
        _audioSource.Play();
    }
    public void PlayDamageAudio()
    {
        _audioSource.clip = _damage;
        _audioSource.Play();
    }
    public void PlaySkillAudio()
    {
        _audioSource.clip = _skill;
        _audioSource.Play();
    }
}
 