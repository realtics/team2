using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void RestartState()
    {
        _currentState.EnterState(_owner);
    }

    // 상태 변경
    public void ChangeState(FSMState<T> newState)
    {
        if (newState == _currentState)
        {
            return;
        }

        _previousState = _currentState;

        if (_currentState != null)
        {
            _currentState.ExitState(_owner);
        }

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.EnterState(_owner);
        }
    }

    public void InitialSetting(T owner, FSMState<T> initialState)
    {
        _owner = owner;
        ChangeState(initialState);
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState(_owner);
        }
    }

    public void StateRevert()
    {
        if (_previousState != null)
        {
            ChangeState(_previousState);
        }
    }
}
