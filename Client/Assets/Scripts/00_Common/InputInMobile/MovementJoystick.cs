using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum JoystickDirection
{
    None,
    Top,
    RightTop,
    Right,
    RightBottom,
    Bottom,
    LeftBottom,
    Left,
    LeftTop
}

public class MovementJoystick : MonoBehaviour
{
    private const float DirectionAngle = 45.0f / 2.0f;

    private Transform _stick;
    private JoystickDirection _stickDirection;
    private JoystickDirection _oldDirection;

    private Vector3 _initPos;
    private float _radius;
    private Vector3 _dirVec;

    private PlayerCharacter _pc;

    void Start()
    {
        _stick = transform.GetChild(0);
        _initPos = _stick.GetComponent<RectTransform>().position;
        _radius = GetComponent<RectTransform>().sizeDelta.x / 3.0f;
        _stickDirection = JoystickDirection.None;
        _oldDirection = _stickDirection;
        _dirVec = Vector2.zero;

        _pc = FindObjectOfType<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerCharacter();
    }

    public void Drag(BaseEventData eventData)
    {
        PointerEventData data = eventData as PointerEventData;
        Vector3 touchPos = data.position;
        _dirVec = CalcStickDirection((touchPos - _initPos).normalized);
        _dirVec = _dirVec.normalized;
        _stick.localPosition = _dirVec * _radius;
    }

    public void DragEnd()
    {
        _stick.localPosition = _initPos;
        _stickDirection = JoystickDirection.None;
        _dirVec = Vector3.zero;
    }

    private void MovePlayerCharacter()
    {
        _pc.SetAxis(_dirVec.x, _dirVec.y);

        _oldDirection = _stickDirection;
    }

    private Vector2 CalcStickDirection(Vector3 direction)
    {
        Vector2 stickDir = Vector2.zero;
        float angle = Quaternion.FromToRotation(Vector3.right, direction).eulerAngles.z;

        if (angle > DirectionAngle && angle < 90.0f - DirectionAngle)
        {
            stickDir = new Vector2(1.0f, 1.0f);
            _stickDirection = JoystickDirection.RightTop;
        }

        else if (angle > 90.0f - DirectionAngle && angle < 135.0f - DirectionAngle)
        {
            stickDir = new Vector2(0.0f, 1.0f);
            _stickDirection = JoystickDirection.Top;
        }
        else if (angle > 135.0f - DirectionAngle && angle < 180.0f - DirectionAngle)
        {

            stickDir = new Vector2(-1.0f, 1.0f);
            _stickDirection = JoystickDirection.LeftTop;
        }

        else if (angle > 180.0f - DirectionAngle && angle < 225.0f - DirectionAngle)
        {
            stickDir = new Vector2(-1.0f, 0.0f);
            _stickDirection = JoystickDirection.Left;
        }

        else if (angle > 225.0f - DirectionAngle && angle < 270.0f - DirectionAngle)
        {
            stickDir = new Vector2(-1.0f, -1.0f);
            _stickDirection = JoystickDirection.LeftBottom;
        }

        else if (angle > 270.0f - DirectionAngle && angle < 315.0f - DirectionAngle)
        {
            stickDir = new Vector2(0.0f, -1.0f);
            _stickDirection = JoystickDirection.Bottom;
        }

        else if (angle > 315.0f - DirectionAngle && angle < 360.0f - DirectionAngle)
        {
            stickDir = new Vector2(1.0f, -1.0f);
            _stickDirection = JoystickDirection.RightBottom;
        }

        else
        {
            stickDir = new Vector2(1.0f, 0.0f);
            _stickDirection = JoystickDirection.Right;
        }

        return stickDir;
    }
}
