using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInactiveEvent : MonoBehaviour
{
    private GameObject _monster;
    private float _currentTime;
    private float _remainTime = 1.0f;
    //TODO Monster.cs 이전
    private bool isDead = false;

    private void Start()
    {
        _monster = transform.root.gameObject;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _remainTime)
                _monster.SetActive(false);
        }

    }

    private void InactiveMonster()
    {
        isDead = true;
    }
}
