using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackBox : MonoBehaviour
{
    private const float AttackRange = 0.8f;
    private Monster _monster;
    private AttackInfoSender _sender;

    void Start()
    {
        _monster = transform.root.GetComponent<Monster>();
        _sender = new AttackInfoSender(transform.root);
        _sender.Damage = 10.0f;
        _sender.HorizontalExtraMoveDuration = 0.2f;
        _sender.HorizontalExtraMoveValue = -15.0f;
        _sender.StunDuration = 1.0f;
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

