using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackBox : MonoBehaviour
{
    private const float AttackRange = 0.8f;
    private Monster _monster;

    void Start()
    {
        _monster = transform.root.GetComponent<Monster>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Player"))
            return;

        if (Mathf.Abs(_monster.transform.position.y - other.transform.position.y) > AttackRange)
            return;

        other.transform.root.GetComponent<BaseUnit>().OnHit(100, transform.root);
    }
}

