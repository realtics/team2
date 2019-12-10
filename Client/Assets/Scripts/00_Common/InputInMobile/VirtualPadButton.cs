using System;
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
	private PlayerCharacter PC
	{
		get
		{
			if (_pc == null)
				_pc = PlayerManager.Instance.PlayerCharacter;

			return _pc;
		}
	}
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
				PC.SetAttack();
                break;
            case VirtualPadButtonType.Jump:
				PC.SetJump();
                break;
            case VirtualPadButtonType.Run:
                if (!_isRun)
                {
                    _pc.SetRun();
                    _isRun = true;
                }
                else
                {
					PC.StopRun();
                    _isRun = false;
                }
                break;
            case VirtualPadButtonType.Skill1:
				PC.SetSkill(SwordmanSkillIndex.Jingongcham);
                break;
            case VirtualPadButtonType.Skill2:
				PC.SetSkill(SwordmanSkillIndex.Hadouken);
                break;
            case VirtualPadButtonType.SkillBlache:
				PC.SetSkill(SwordmanSkillIndex.Blache);
                break;
        }
    }
}
