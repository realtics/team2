using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackBox : MonoBehaviour
{
    protected const float AttackRange = 0.8f;
    protected BaseMonster _monster;
    [SerializeField]
    protected AttackInfoSender _sender;
    [SerializeField]
    protected float _damgePercent;

    protected virtual void Start()
    {
        _monster = transform.root.GetComponent<BaseMonster>();
        _sender.Attacker = transform.root;
        _sender.Damage = _monster.AttackDamage * _damgePercent;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Player"))
            return;

        if (Mathf.Abs(_monster.transform.position.y - other.transform.position.y) > AttackRange)
            return;
        other.transform.root.GetComponent<BaseUnit>().OnHit(_sender);
    }
}

