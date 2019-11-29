using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackBox : MonoBehaviour
{
    private const float AttackRange = 1.2f;
    private CharacterStat _stat;

    [SerializeField]
    private AttackInfoSender _sender;

    void Start()
    {
        _stat = transform.root.GetComponent<CharacterStat>();

        _sender.Attacker = transform.root;
        _sender.Damage = _stat.AttackDamage;
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("MonsterHitBox"))
            return;

        if (Mathf.Abs(_stat.transform.position.y - other.transform.root.position.y) > AttackRange)
            return;

        _sender.Damage = _stat.AttackDamage;
        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);

    }
}
