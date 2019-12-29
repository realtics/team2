using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCryAttackBox : MonoBehaviour
{
    [SerializeField]
    private AttackInfoSender _sender;

    private void Awake()
    {
        _sender.Attacker = transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("MonsterHitBox"))
            return;

        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);
    }
}
