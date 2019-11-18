using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackBox : MonoBehaviour
{
    private const float AttackRange = 0.8f;
    private BaseUnit _unit;
    void Start()
    {
        _unit = transform.root.GetComponent<BaseUnit>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Monster"))
            return;

        if (Mathf.Abs(_unit.transform.position.y - other.transform.position.y) > AttackRange)
            return;

        other.transform.root.GetComponent<Monster>().OnHit(_unit.Stat.AttackDamage);

    }
}
