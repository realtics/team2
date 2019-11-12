using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * StateMachine.cs
 * 출처 : https://carrotclub.tistory.com/entry/%EB%AA%AC%EC%8A%A4%ED%84%B0-FSM-%EA%B5%AC%ED%98%84
 * 몬스터 클래스에서 각 몬스터상태 클래스를 변경할 수 있도록 연결해주는 역할
 * ##공부중##
 */

public class StateMachine<T>
{
    private T _owner;
    private FSMState<T> _currentState;
    private FSMState<T> _previousState;

    // 변수 초기화
    public void Awake()
    {
        _currentState = null;
        _previousState = null;
    }

    // 상태 변경
    public void ChangeState(FSMState<T> _NewState)
    {
        // 같은 상태를 변환하려 한다면 나감
        if (_NewState == _currentState)
        {
            return;
        }

        _previousState = _currentState;

        // 현재 상태가 있다면 종료
        if (_currentState != null)
        {
            _currentState.ExitState(_owner);
        }

        _currentState = _NewState;

        // 새로 적용된 상태가 null이 아니면 실행
        if (_currentState != null)
        {
            _currentState.EnterState(_owner);
        }
    }

    // 초기상태설정
    public void InitialSetting(T owner, FSMState<T> _InitialState)
    {
        _owner = owner;
        ChangeState(_InitialState);
    }

    // 상태 업데이트
    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState(_owner);
        }
    }

    // 이전 상태로 회귀
    public void StateRevert()
    {
        if (_previousState != null)
        {
            ChangeState(_previousState);
        }
    }
}
