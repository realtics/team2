using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VirtualPadButtonType
{
    Attack,
    Jump,
    Run,
}

public class VirtualPadButton : MonoBehaviour
{
    [SerializeField]
    private VirtualPadButtonType _type;
    private PlayerCharacter _pc;
    private bool _isRun;

    private void Start()
    {
        _pc = FindObjectOfType<PlayerCharacter>();
        _isRun = false;
    }

    public void OnClick()
    {
        ButtonAction();
    }

    public void ButtonAction()
    {
        switch (_type)
        {
            case VirtualPadButtonType.Attack:
                _pc.SetAttack();
                break;
            case VirtualPadButtonType.Jump:
                _pc.SetJump();
                break;
            case VirtualPadButtonType.Run:
                if (!_isRun)
                {
                    _pc.SetRun();
                    _isRun = true;
                }
                else
                {
                    _pc.StopRun();
                    _isRun = false;
                }
                break;
        }
    }
}
