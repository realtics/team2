using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackBox : MonoBehaviour
{
    private const float AttackRange = 0.8f;
    private CharacterStat _stat;
    private AttackInfoSender _sender;

    void Start()
    {
        _stat = transform.root.GetComponent<CharacterStat>();

        _sender = new AttackInfoSender(transform.root);
        _sender.Damage = _stat.AttackDamage;
        _sender.HorizontalExtraMoveDuration = 0.2f;
        _sender.HorizontalExtraMoveValue = -2.0f;
        _sender.StunDuration = 1.0f;
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Monster"))
            return;

        if (Mathf.Abs(_stat.transform.position.y - other.transform.position.y) > AttackRange)
            return;


        other.transform.root.GetComponent<Monster>().OnHit(_sender);

    }
}
