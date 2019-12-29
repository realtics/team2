using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Calvary : BaseMonster
{
    private enum CalvaryAttackMotion
    {
        AttackMotion1 = 0,
        AttackMotion2 = 1,
        AttacknMotionEnd = 2
    }
    private CalvaryAttackMotion _currentAttackMotion;

    [SerializeField]
    private float _smashAttackResetTime;
    [SerializeField]
    private float _smashAttackCurrentTime;

    [SerializeField]
    private Transform _smashAttackBox;

    private AudioSource _audioSource;
    [Space]
    [SerializeField]
    private AudioClip _meet;
    [SerializeField]
    private AudioClip _madMode;
    [SerializeField]
    private AudioClip _baseAttack;
    [SerializeField]
    private AudioClip _smashAttack;

    [Space]
    [SerializeField]
    private Transform _madBackEffect;
    [SerializeField]
    private Transform _madFrontEffect;

    private bool _isMadeMode = false;
    private float _originalRange;
    private bool _CheckTrnasition =false;

    protected override void SetInitialState()
    {
        base.SetInitialState();
        _originalRange = _attackRange;
        _attackRange = _originalRange * 3;
        _smashAttackCurrentTime = _smashAttackResetTime;
    }

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        OnSuperArmor();
    }

    private void Start()
    {
        AudioPlay(_meet);
        _audioSource.Play();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_isMadeMode && _currentHp <= _maxHp / 2)
        {
            ChangeMadMode();
        }
        ChangeAttackRange();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //MonsterManager.Instance.ReceiveMonsterDie(this);
            ChangeDieState();
        }
#endif
    }

    public void ActiveSmashAttackBox()
    {
        _smashAttackBox.gameObject.SetActive(true);
    }

    public void InactiveSmashAttackBox()
    {
        _smashAttackBox.gameObject.SetActive(false);
    }

    //AttackState
    public override void EnterAttackState()
    {
        base.EnterAttackState();
        CanSmashAttack();
        _animator.SetInteger("attackMotion", (int)_currentAttackMotion);
    }

    public override void UpdateAttackState()
    {
        if (!_animator.IsInTransition(0) && !_CheckTrnasition)
        {
            if (_currentAttackMotion == CalvaryAttackMotion.AttackMotion2)
                AudioPlay(_smashAttack);
            else if (_currentAttackMotion == CalvaryAttackMotion.AttackMotion1)
                AudioPlay(_baseAttack);
           
            _CheckTrnasition = true;
        }
        base.UpdateAttackState();
    }

    public override void ExitAttackState()
    {
        base.ExitAttackState();
        _animator.SetFloat("animSpeed", 1.0f);
        _attackRange = _originalRange;
        _CheckTrnasition = false;
    }

    //MoveState
    public override void EnterMoveState()
    {
        base.EnterMoveState();
    }

    public override void UpdateMoveState()
    {
        base.UpdateMoveState();
    }

    public override void ExitMoveState()
    {
        base.ExitMoveState();
    }

    //HitState
    public override void EnterHitState()
    {
        base.EnterHitState();
    }

    public override void UpdateHitState()
    {
        base.UpdateHitState();
    }

    public override void ExitHitState()
    {
        base.ExitHitState();
    }

    //DieState
    public override void EnterDieState()
    {
        if (_madBackEffect.gameObject.activeSelf)
        {
            _madBackEffect.gameObject.SetActive(false);
            _madFrontEffect.gameObject.SetActive(false);
        }
        OffSuperArmor();

        StartCoroutine(TimeScaleSlow());
        EffectManager.Instance.SpawnClearCircle(HitBoxCenter);

        base.EnterDieState();
    }

    public override void UpdateDieState()
    {
        base.UpdateDieState();
    }

    public override void ExitDieState()
    {
        base.ExitDieState();
    }

    protected override void SetAerialValue(AttackInfoSender sender)
    {
        //FIXME : 컴포넌트화?
        //this monster don't have aerialvalue
        return;
    }

    public override void NoticeDie()
    {
        MonsterManager.Instance.ReceiveBossMonsterDie(this);
    }

    private IEnumerator TimeScaleSlow()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }

    private void CanSmashAttack()
    {
        if (_smashAttackCurrentTime >= _smashAttackResetTime)
        {
            _currentAttackMotion = CalvaryAttackMotion.AttackMotion2;
            StartCheckSmashAttackTime();
        }
        else
        {
            _currentAttackMotion = CalvaryAttackMotion.AttackMotion1;
            _animator.SetFloat("animSpeed", 1.5f);
        }
    }

    IEnumerator CheckSmashAttackTime()
    {
        _smashAttackCurrentTime += Time.deltaTime;

        if (_smashAttackCurrentTime >= _smashAttackResetTime)
        {
            StopCoroutine("CheckSmashAttackTime");
            yield break;
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine("CheckSmashAttackTime");
    }

    protected void StartCheckSmashAttackTime()
    {
        _smashAttackCurrentTime = 0.0f;
        StartCoroutine("CheckSmashAttackTime");
    }

    private void ChangeAttackRange()
    {
        if (_smashAttackCurrentTime >= _smashAttackResetTime)
        {
            _attackRange = _originalRange * 3;
        }
    }

    private void ChangeMadMode()
    {
        AudioPlay(_madMode);

        _madBackEffect.gameObject.SetActive(true);
        _madFrontEffect.gameObject.SetActive(true);

        _attackDamage *= 2f;
        _defensePercent += 0.25f;
        _moveSpeed *= 1.5f;

        _isMadeMode = true;
    }

    private void AudioPlay(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}

