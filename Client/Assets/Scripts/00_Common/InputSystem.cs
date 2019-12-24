using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter _pc;
    private float _axisVertical;
    private float _axisHorizontal;

    public PlayerCharacter PC { get { return _pc; } set { _pc = value; } }

    private void Awake()
    {
        _pc = FindObjectOfType<PlayerCharacter>();
    }

    private void Update()
    {
        MoveKeyProcess();
    }

    private void MoveKeyProcess()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            _axisHorizontal = -1.0f;
        else if (Input.GetKey(KeyCode.RightArrow))
            _axisHorizontal = 1.0f;
        else
            _axisHorizontal = 0.0f;

        if (Input.GetKey(KeyCode.UpArrow))
            _axisVertical = 1.0f;
        else if (Input.GetKey(KeyCode.DownArrow))
            _axisVertical = -1.0f;
        else
            _axisVertical = 0.0f;

        _pc.SetAxis(_axisHorizontal, _axisVertical);

        if (Input.GetKeyDown(KeyCode.C))
            _pc.SetJump();

        if (Input.GetKeyDown(KeyCode.X))
            _pc.SetAttack();

        if (Input.GetKeyDown(KeyCode.A))
            _pc.SetSkill(SwordmanSkillIndex.Hadouken);
        if (Input.GetKeyDown(KeyCode.S))
            _pc.SetSkill(SwordmanSkillIndex.Jingongcham);
        if (Input.GetKeyDown(KeyCode.R))
            _pc.SetSkill(SwordmanSkillIndex.Blache);

        if (Input.GetKey(KeyCode.LeftShift))
            _pc.SetRun();
        else
            _pc.StopRun();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackInfoSender sender = new AttackInfoSender();
            sender.Attacker = transform;
            sender.ExtraHeightValue = 0.2f;
            sender.HorizontalExtraMoveDuration = 10.0f;
            sender.HorizontalExtraMoveValue = -15.0f;
            sender.StunDuration = 0.5f;
            _pc.SetTest(sender);
        }

		if (Input.GetKeyDown(KeyCode.V))
		{
			_pc.Revive();
		}
    }

    public void FindPlayerCharacter()
    {
        _pc = FindObjectOfType<PlayerCharacter>();
        _pc.FindMovement();
    }
}
