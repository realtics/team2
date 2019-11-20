using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter _pc;
    private float _axisVertical;
    private float _axisHorizontal;

    private void Awake()
    {
        //_pc = FindObjectOfType<PlayerCharacter>();
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

        if (Input.GetKey(KeyCode.LeftShift))
            _pc.SetRun();
        else
            _pc.StopRun();
    }
}
