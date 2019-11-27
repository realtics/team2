using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CharacterSpawnData
{
    public int id;
    public Vector3 position;
}
public class Character : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    private int _id;
    private Rigidbody _rigidBody;
    private bool _isMoving;

    private Vector3 _moveDirection;
    private Vector3 _oldDirection;

    public bool IsMine { get { return _id == NetworkManager.Instance.GetMyId; } }
    public int Id { get { return _id; } }
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _moveDirection = Vector3.zero;
        _oldDirection = _moveDirection;
    }

    void Update()
    {
        KeyInputProcess();
    }

    // 이 스크립트는 캐릭터의 움직임을 담당하는 곳이기 때문에
    // 키를 입력 받는 로직은 스크립트를 따로 두어 이 스크립트를 참조하는 식이 좋다.
    // 여기 쓰면 안된다.
    private void KeyInputProcess()
    {
        if (!IsMine)
            return;

        // 수평 수직의 값을 받는 함수
        // LeftArrow, RightArrow, A, D가 Horizontal
        // UpArrow, DownArrow, W, S가 Vertical를 반환한다.
        // 이 옵션은 ProjectSetting -> Input에서 수정 가능하다.

        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector3(horizontalAxis, 0.0f, verticalAxis);

        if (_moveDirection != Vector3.zero)
        {
            SetMoveDirectionAndMove(transform.position, _moveDirection);
            SendPacketMoveStart();
        }
        else
        {
            SendPacketMoveEnd();
            
        }
            
    }

    private void SendPacketMoveStart()
    {
        if (!_isMoving)
            return;

        if (_oldDirection == _moveDirection)
            return;

        Debug.Log("SendMoveStart");
        _oldDirection = _moveDirection;

        NetworkManager.Instance.MoveStart(transform.position, _moveDirection);
    }

    private void SendPacketMoveEnd()
    {
        if (!_isMoving)
            return;

        Debug.Log("SendMoveEnd");

        NetworkManager.Instance.MoveEnd(transform.position);

        StopMove(transform.position);
    }



    private void FixedUpdate()
    {
        CharacterMovementProcess();
    }

    private void CharacterMovementProcess()
    {
        if (!_isMoving)
            return;

        // 좌표를 움직이는 함수.
        // 실제 좌표를 때려넣는 식이기 때문에 물리에 영향을 받지 않는다.
        transform.position = Vector3.MoveTowards(transform.position, 
                                                 transform.position + _moveDirection, _speed * Time.deltaTime);
    }

    // 서버로부터 받은 MoveStart와 같은 패킷의 MoveDirection을 세팅한다.
    public void SetMoveDirectionAndMove(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        _moveDirection = dir;
        _isMoving = true;

        transform.LookAt(transform.position + _moveDirection);
    }
    
    // MoveEnd와 같은 패킷을 받아 호출하는 함수
    // 인자는 서버로부터 보낸 이 캐릭터의 마지막 좌표
    public void StopMove(Vector3 lastPosition)
    {
        // 이렇게 바로 포지션을 넣으면 오차가 많이 발생했을 때 순간이동하는 것 처럼 보일 수 있다.
        // 보간이 필요한 로직
        transform.position = lastPosition;
        _isMoving = false;
    }

    public void SetId(int id)
    {
        _id = id;
    }
}
