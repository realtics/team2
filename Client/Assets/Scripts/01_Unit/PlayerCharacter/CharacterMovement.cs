using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterMovePacket
{
	public Vector3 position;
	public Vector3 direction;
}

public class CharacterMovement : BaseUnit
{
    private int _id;
	private string _nickName;
    private bool _nextAttack;
    private CharacterAnimController _animController;
    private Dictionary<SwordmanSkillIndex, CharacterSkill> _equipSkills;
    private SwordmanSkillIndex _usedSkill;

	private bool _onMoveStartPacket;
	private bool _onMoveEndPacket;

	private CharacterMovePacket _moveStartPacket;
	private CharacterMovePacket _moveEndPacket;

    public CharacterSkill UsedSkill { get { return _equipSkills[_usedSkill]; } }
    public int Id { get { return _id; } set { _id = value; } }
	public string NickName { get { return _nickName; } set { _nickName = value; } }
    public bool IsMine { get { return NetworkManager.Instance.MyId == Id ? true : false; } }


    protected override void Awake()
    {
        base.Awake();
        _nextAttack = false;
        _animator.SetBool("IsGround", true);
        _animator.SetBool("NextAttack", false);
        _animController = GetComponentInChildren<CharacterAnimController>();
    }

	protected override void Start()
	{
		_equipSkills = new Dictionary<SwordmanSkillIndex, CharacterSkill>();
		_equipSkills.Add(SwordmanSkillIndex.Jingongcham, SwordmanSkillManager.Instance.GetSkill(_stat, SwordmanSkillIndex.Jingongcham));
		_equipSkills.Add(SwordmanSkillIndex.Hadouken, SwordmanSkillManager.Instance.GetSkill(_stat, SwordmanSkillIndex.Hadouken));
		_equipSkills.Add(SwordmanSkillIndex.Blache, SwordmanSkillManager.Instance.GetSkill(_stat, SwordmanSkillIndex.Blache));
	}

	protected override void Update()
    {
        CheckAttackEnd();
        SkillCoolTimeUpdate();
		ProcessPacketMoveStart();
		ProcessPacketMoveEnd();
	}

    private void SkillCoolTimeUpdate()
    {
        foreach (CharacterSkill skill in _equipSkills.Values)
        {
            skill.UpdateCoolTime();
        }
    }

    private void CheckAttackEnd()
    {
        //if (!IsAttack)
        //    return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!animStateInfo.IsTag("Attack"))
            return;

        if (IsAnimationPlaying())
            return;

        StopAttack();
    }

    public override void StopAttack()
    {
        _animController.OffAttackBox();
        if (IsInTranstion)
            return;

        base.StopAttack();
        StopSkill();

        _animator.SetBool("IsAttack", false);
        _animator.SetBool("NextAttack", false);
    }

    public override bool SetAttack()
    {
        if (!base.SetAttack())
            return false;

		_animator.SetBool("IsMoving", false);
		_animator.SetBool("IsAttack", true);
        SetNextAttack();

        return true;
    }

    public void SetNextAttack()
    {
        if (!IsAttack)
            return;

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (!animStateInfo.IsTag("Attack"))
            return;

        if (animStateInfo.IsName("Attack3"))
            return;

		if (!IsGround)
			return;

        if (CurAnimTime < 0.5f)
            return;

        if (!IsAnimationPlaying())
            return;

        if (IsInTranstion)
            return;

        MoveUnit(1.0f, 0.3f, ExtraMoveDirection.Horizontal);

        _animator.SetBool("NextAttack", true);
        StartCoroutine(CoSetFalseNextAttack());
		_animController.OffAttackBox();
	}

    public override bool SetJump()
    {
        if (!base.SetJump())
            return false;

        _animator.SetBool("IsJumpUp", true);
        _animator.SetBool("IsGround", false);

        return true;
    }

    public override bool SetAirHitHeight(float power)
    {
        if (!base.SetAirHitHeight(power))
            return false;

        _animator.SetBool("IsGround", false);
        _animator.SetBool("IsAirHit", true);
        return true;
    }

    protected override bool SetJumpDown()
    {
        if (!base.SetJumpDown())
            return false;

        _animator.SetBool("IsJumpUp", false);
        _animator.SetBool("IsJumpDown", true);

        return true;
    }

    public override bool SetRun()
    {
        if (!base.SetRun())
            return false;

        _animator.SetBool("IsRun", true);

        return true;
    }


    public override bool StopRun()
    {
        if (!base.StopRun())
            return false;

        _animator.SetBool("IsRun", false);

        return true;
    }

    protected override bool CheckGroundAfterJump()
    {
        if (!base.CheckGroundAfterJump())
            return false;

        _animator.SetBool("IsJumpDown", false);
        _animator.SetBool("IsGround", true);

        return true;
    }

    public override bool SetAxis(float horizontal, float vertical)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
            return false;

        if (!base.SetAxis(horizontal, vertical))
            return false;

        _animator.SetBool("IsMoving", IsMoving);

        return true;
    }

    public override bool OnHit(AttackInfoSender sender)
    {
        if (!base.OnHit(sender))
            return false;

        _animator.SetBool("IsHit", true);

        if (sender.Attacker.position.x > transform.position.x)
            SetFlipX(false);
        else
            SetFlipX(true);

        if (sender.HorizontalExtraMoveDuration > 0.0f)
            MoveUnit(sender.HorizontalExtraMoveValue, sender.HorizontalExtraMoveDuration, ExtraMoveDirection.Horizontal);

        if (sender.VerticalExtraMoveDuration > 0.0f)
            MoveUnit(sender.VerticalExtraMoveValue, sender.VerticalExtraMoveDuration, ExtraMoveDirection.Vertical);

        if (sender.ExtraHeightValue > 0.0f)
            SetAirHitHeight(sender.ExtraHeightValue);

        return true;
    }

    public override void SetHit(float stunDuration)
    {
        base.SetHit(stunDuration);
        SetRecoveryTime(stunDuration);
    }

    public override void StopHit()
    {
        base.StopHit();
        _animator.SetBool("IsHit", false);
        _animator.SetBool("IsAirHit", false);
        _animator.SetBool("IsAttack", false);
    }

    public override bool SetSkill(SwordmanSkillIndex skill)
    {
        _usedSkill = skill;

        if (!UsedSkill.UsableSkill)
        {

            _usedSkill = SwordmanSkillIndex.None;
            return false;
        }

		if (!CheckIsAttack())
			return false;

		if (!base.SetSkill(skill))
            return false;

		_animator.SetBool("IsMoving", false);
        _animator.SetBool("OnSkill", true);
        _animator.SetInteger("SkillMotion", UsedSkill.MotionIndex);

        return true;
    }

    public override void StopSkill()
    {
		base.StopSkill();
        _animator.SetBool("OnSkill", false);
    }

    IEnumerator CoSetFalseNextAttack()
    {
        yield return new WaitForEndOfFrame();
        _animator.SetBool("NextAttack", false);
    }

    public override void OnSkill()
    {
        UsedSkill.OnSkill();
    }

    private bool CheckIsAttack()
    {
        if (!IsAttack)
            return true;

		if (CurAnimTime < 0.2f)
			return false;

        StopAttack();

        return true;
    }

	private void ProcessPacketMoveStart()
	{
		if (!_onMoveStartPacket)
			return;

		transform.position = _moveStartPacket.position;
		SetAxis(_moveStartPacket.direction.x, _moveStartPacket.direction.y);

		if (_moveStartPacket.direction.x < 0.0f)
			SetFlipX(true);
		else if (_moveStartPacket.direction.x > 0.0f)
			SetFlipX(false);

		_onMoveStartPacket = false;
	}

	private void ProcessPacketMoveEnd()
	{
		if (!_onMoveEndPacket)
			return;

		Vector3 normal = Vector3.Normalize(_moveEndPacket.position - transform.position);
		SetAxis(normal.x, normal.y);

		if (Vector3.Normalize(_moveEndPacket.position - transform.position).x > 0)
			SetFlipX(false);
		else
			SetFlipX(true);

		if (Vector3.SqrMagnitude(_moveEndPacket.position - transform.position) <= 0.01f)
		{
			SetAxis(0, 0);
			_onMoveEndPacket = false;
		}
	}

    public void SetMoveDirectionAndMove(Vector3 pos, Vector3 dir)
    {
		//_onMoveStartPacket = true;

		_moveStartPacket = new CharacterMovePacket();
		_moveStartPacket.position = pos;
		_moveStartPacket.direction = dir;
	}

    public void StopMove(Vector3 pos)
    {
		_onMoveEndPacket = true;

		_moveEndPacket = new CharacterMovePacket();
		_moveEndPacket.position = pos;
		_moveEndPacket.direction = Vector3.zero;
	}

	public override void SetDie()
	{
		base.SetDie();
		_animator.SetBool("IsHit", true);
		_animator.SetBool("IsDie", true);
	}

	public CharacterSkill GetEquipSkill(SwordmanSkillIndex index)
	{
		CharacterSkill skill = null;
		skill = _equipSkills[index];
		return skill;
	}
}
