using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacheAttackBox : MonoBehaviour
{
    private AttackInfoSender _sender;

    void Start()
    {
        _sender = new AttackInfoSender();
        _sender.Damage = 99999.0f;
        _sender.StunDuration = 1.0f;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("MonsterHitBox"))
            return;

        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);
    }
}
