using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacheAttackBox : MonoBehaviour
{
    private AttackInfoSender _sender;

    public void SetAttackInfo(AttackInfoSender info)
    {
        _sender = info;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("MonsterHitBox"))
            return;

        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);
    }
}
