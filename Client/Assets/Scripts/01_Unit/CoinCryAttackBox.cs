using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCryAttackBox : MonoBehaviour
{
    [SerializeField]
    private AttackInfoSender _sender;
    [SerializeField]
    private BoxCollider2D _collider;

    private float _deadTime;

    private void Awake()
    {
        _sender.Attacker = transform;
    }

    private void OnValidate()
    {
        _deadTime = 1.0f;
        _collider.enabled = true;
    }

    private void Update()
    {
        if (_deadTime <= 0.0f)
            gameObject.SetActive(false);
        if (_deadTime >= 0.1f)
            _collider.enabled = false;

        _deadTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("MonsterHitBox"))
            return;

        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);
    }
}
