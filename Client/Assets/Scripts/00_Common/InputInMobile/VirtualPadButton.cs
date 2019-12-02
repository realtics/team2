﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VirtualPadButtonType
{
    Attack,
    Jump,
    Run,
    Skill1,
    Skill2,
    SkillBlache,
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
            case VirtualPadButtonType.Skill1:
                _pc.SetSkill(SwordmanSkillIndex.Jingongcham);
                break;
            case VirtualPadButtonType.Skill2:
                _pc.SetSkill(SwordmanSkillIndex.Hadouken);
                break;
            case VirtualPadButtonType.SkillBlache:
                _pc.SetSkill(SwordmanSkillIndex.Blache);
                break;
        }
    }
}
