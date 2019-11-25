using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    private GameObject _attackBox;
    private BaseUnit _movement;

    private void Start()
    {
        _attackBox = transform.GetChild(0).gameObject;
        _attackBox.SetActive(false);
        _movement = transform.root.GetComponent<BaseUnit>();
    }

    private void Update()
    {
    }

    public void OnAttackBox()
    {
        if (!_movement.IsAttackable())
            return;

        _attackBox.SetActive(true);
    }

    public void OffAttackBox()
    {
        _attackBox.SetActive(false);
    }

    public void OnSkill()
    {
        _movement.OnSkill();
    }
}
