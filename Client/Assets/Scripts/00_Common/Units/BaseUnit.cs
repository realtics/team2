using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitJumpState
{
    None = 0,
    JumpUp = 1,
    JumpDown = -1,
}

public class BaseUnit : MonoBehaviour
{
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


    // properties
    public bool IsGround { get { return !(_height > 0.0f); } }
    public bool IsRun { get { return _isRun; } }
    public bool IsAttack { get { return _isAttack; } }
    public bool IsMoving { get { return (Mathf.Abs(_axisHorizontal) > 0.0f || Mathf.Abs(_axisVertical) > 0.0f); } }
    public bool IsJumping { get { return _jumpState != UnitJumpState.None; } }
    public bool IsJumpUp { get { return _jumpState == UnitJumpState.JumpUp; } }
    public bool IsJumpDown { get { return _jumpState == UnitJumpState.JumpDown; } }
    public bool IsHit { get { return _isHit; } }

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
        _originPos = _avatar.localPosition;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        MoveProcess();
        JumpProcess();
    }

    protected virtual void MoveProcess()
    {
        Vector3 velocity = _rgdBody.velocity;

        velocity.x = _axisHorizontal * _speedHorizontal;

        if (_isRun)
            velocity.x *= 2.0f;

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
            _jumpValue -= 0.01f;

            if (_jumpValue < 0.0f)
                _jumpState = UnitJumpState.JumpDown;

            if (_height < 0.0f)
            {
                _jumpState = UnitJumpState.None;
                _height = 0.0f;
                StopAttack();
            }
        }
    }

    public void SetAxis(float horizontal, float vertical)
    {
        if (!IsMovable())
        {
            _axisHorizontal = 0.0f;
            _axisVertical = 0.0f;
            return;
        }

        _axisHorizontal = horizontal;
        _axisVertical = vertical;
    }

    public virtual void SetAttack()
    {
        if (!IsAttackable())
            return;

        _isAttack = true;
    }

    public virtual void StopAttack()
    {
        _isAttack = false;
    }

    public void SetRun()
    {
        if (!IsGround)
            return;

        _isRun = true;
    }

    public void StopRun()
    {
        if (!IsGround)
            return;

        _isRun = false;
    }

    public void SetJump()
    {
        if (!IsJupable())
            return;

        _jumpState = UnitJumpState.JumpUp;
        _jumpValue = 0.25f;
    }

    public void SetFlipX(bool flip)
    {
        _renderer.flipX = flip;
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
        if (IsAttack)
            return false;

        return true;
    }
}
