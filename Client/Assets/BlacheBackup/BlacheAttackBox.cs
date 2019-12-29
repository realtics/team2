using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacheAttackBox : MonoBehaviour
{
    private AttackInfoSender _sender;

    void Start()
    {
        _sender = new AttackInfoSender();
        _sender.Damage = 6666.0f;
        _sender.ExtraHeightValue = 0.2f;
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
