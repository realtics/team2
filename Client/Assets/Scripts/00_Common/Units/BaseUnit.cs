using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitJumpState
{
    None = 0,
    JumpUp = 1,
    JumpDown = -1,
}

public enum ExtraMoveDirection
{
    Horizontal = 0,
    Vertical,
}

public class BaseUnit : MonoBehaviour
{
    protected CharacterStat _stat;
    private const float RunSpeedMultiple = 2.0f;
    private const float JumpGravity = 0.01f;
    private const float JumpPower = 0.25f;

    protected Animator _animator;
    protected SpriteRenderer _renderer;
    protected Rigidbody2D _rgdBody;

    [SerializeField]
    protected Transform _avatar;
    protected Vector3 _originPos;

    private float _axisHorizontal;
    private float _axisVertical;
    private UnitJumpState _jumpState;
    private float _jumpValue;

    private bool _isAttack;
    private bool _isRun;

    private float _speedHorizontal;
    private float _extraSpeedHorizontal;

    private float _speedVertical;
    private float _extraSpeedVertical;

    private float _height;
    private bool _isHit;
    private float _hitRecoveryTime;

    private float _extraMoveDuration;


    // properties
    public bool IsGround { get { return !(_height > 0.0f); } }
    public bool IsRun { get { return _isRun; } }
    public bool IsAttack { get { return _isAttack; } }
    public bool IsMoving { get { return (Mathf.Abs(_axisHorizontal) > 0.0f || Mathf.Abs(_axisVertical) > 0.0f); } }
    public bool IsJumping { get { return _jumpState != UnitJumpState.None; } }
    public bool IsJumpUp { get { return _jumpState == UnitJumpState.JumpUp; } }
    public bool IsJumpDown { get { return _jumpState == UnitJumpState.JumpDown; } }
    public bool IsHit { get { return _isHit; } }
    public float Forward { get { return _avatar.transform.localScale.x < 0 ? -1 : 1; } }
    public float CurAnimTime { get { return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime; } }
    public bool IsInTranstion { get { return _animator.IsInTransition(0); } }

    public Vector3 OriginPos { get { return _avatar.position; } }
    public CharacterStat Stat { get { return _stat; } }

    protected virtual void Awake()
    {
        _axisHorizontal = 0.0f;
        _axisVertical = 0.0f;
        _speedHorizontal = 3.0f;
        _speedVertical = 3.0f;
    }

    protected virtual void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _rgdBody = GetComponent<Rigidbody2D>();
        _stat = GetComponent<CharacterStat>();
        _originPos = _avatar.localPosition;
    }

    protected virtual void Update()
    {
        FindObjectsOfType<GameObject>();
    }

    protected virtual void FixedUpdate()
    {
        MoveProcess();
        JumpProcess();
        RecoveryHit();
        ExtraMoveTimeProcess();
    }

    protected virtual void RecoveryHit()
    {
        if (!_isHit)
            return;

        _hitRecoveryTime -= Time.deltaTime;

        if (_hitRecoveryTime <= 0.0f)
        {
            StopHit();
        }
    }

    protected virtual void MoveProcess()
    {
        Vector3 velocity = _rgdBody.velocity;

        velocity.x = _axisHorizontal * _speedHorizontal;

        if (_isRun)
            velocity.x *= RunSpeedMultiple;

        velocity.x += _extraSpeedHorizontal;

        velocity.y = _axisVertical * _speedVertical;
        velocity.y += _extraSpeedVertical;

        _rgdBody.velocity = velocity;
    }

    protected virtual void JumpProcess()
    {
        Vector3 groundPos = _originPos;
        groundPos.y += _height;
        _avatar.localPosition = groundPos;

        if (_jumpState != UnitJumpState.None)
        {
            _height += _jumpValue;
            _jumpValue -= JumpGravity;

            SetJumpDown();
            CheckGroundAfterJump();
        }
    }

    public virtual bool SetAxis(float horizontal, float vertical)
    {
        if (!IsMovable())
        {
            _axisHorizontal = 0.0f;
            _axisVertical = 0.0f;
            return false;
        }

        _axisHorizontal = horizontal;
        _axisVertical = vertical;
        return true;
    }

    public virtual bool SetAttack()
    {
        if (!IsAttackable())
            return false;

        _isAttack = true;
        return true;
    }

    public virtual void StopAttack()
    {
        _isAttack = false;
    }

    public virtual bool SetRun()
    {
        if (!IsGround)
            return false;

        _isRun = true;
        return true;
    }

    public virtual bool StopRun()
    {
        if (!IsGround)
            return false;

        _isRun = false;
        return true;
    }

    public virtual bool SetJump()
    {
        if (!IsJupable())
            return false;

        _jumpState = UnitJumpState.JumpUp;
        _jumpValue = JumpPower;
        return true;
    }

    protected virtual bool SetJumpDown()
    {
        if (_jumpValue >= 0.0f)
            return false;

        _jumpState = UnitJumpState.JumpDown;

        return true;
    }

    protected virtual bool CheckGroundAfterJump()
    {
        if (_height >= 0.0f)
            return false;

        _jumpState = UnitJumpState.None;
        _height = 0.0f;
        StopAttack();

        return true;
    }

    public virtual void SetFlipX(bool flip)
    {
        Vector3 scale = _avatar.transform.localScale;

        if (flip)
        {
            if (scale.x > 0)
                scale.x = -scale.x;
        }
        else
        {
            if (scale.x < 0)
                scale.x = -scale.x;
        }
        _avatar.localScale = scale;
        //_renderer.flipX = flip;
    }

    public bool IsMovable()
    {
        if (_isHit)
            return false;

        if (IsGround)
        {
            if (_isAttack)
                return false;
        }

        return true;
    }

    public bool IsAttackable()
    {
        if (_isHit)
            return false;

        return true;
    }

    public bool IsJupable()
    {
        if (IsHit)
            return false;
        if (IsAttack)
            return false;

        return true;
    }

    public void MoveUnit(float power, float duration, ExtraMoveDirection direction)
    {
        if (direction == ExtraMoveDirection.Horizontal)
            _extraSpeedHorizontal = power * Forward;
        else
            _extraSpeedVertical = power * Forward;

        _extraMoveDuration = duration;
    }

    private void ExtraMoveTimeProcess()
    {
        if (_extraMoveDuration <= 0.0f)
            return;

        _extraMoveDuration -= Time.deltaTime;

        if (_extraMoveDuration <= 0.0f)
        {
            _extraSpeedHorizontal = 0.0f;
            _extraSpeedVertical = 0.0f;
            _extraMoveDuration = 0.0f;
            return;
        }
    }

    public virtual void SetHit()
    {
        _isHit = true;
    }

    public virtual void StopHit()
    {
        _isHit = false;
        _hitRecoveryTime = 0.0f;
    }

    public bool IsAnimationPlaying()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.95f;
    }

    public virtual void OnHit(float damage)
    {
        Debug.Log("UnitOnHit");
        SetHit();
        StopAttack();
        _stat.OnHitDamage(damage);
    }

    protected void SetRecoveryTime(float time)
    {
        _hitRecoveryTime = time;
    }
}
