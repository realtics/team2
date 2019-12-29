using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalvarySmashCircleBox : MonsterAttackBox
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Player"))
            return;

        if (Mathf.Abs(_monster.transform.position.y - other.transform.position.y) > AttackRange)
            return;
        if (!other.transform.root.GetComponent<BaseUnit>().IsGround)
            return;

        other.transform.root.GetComponent<BaseUnit>().OnHit(_sender);
    }
}
