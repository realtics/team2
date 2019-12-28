using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackBox : MonoBehaviour
{
    private const float AttackRange = 0.8f;
    private BaseMonster _monster;
    [SerializeField]
    private AttackInfoSender _sender;
    [SerializeField]
    private float _damgePercent;

    private void Start()
    {
        _monster = transform.root.GetComponent<BaseMonster>();
        _sender.Attacker = transform.root;
        _sender.Damage = _monster.AttackDamage * _damgePercent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Player"))
            return;

        if (Mathf.Abs(_monster.transform.position.y - other.transform.position.y) > AttackRange)
            return;

        other.transform.root.GetComponent<BaseUnit>().OnHit(_sender);
    }
}

