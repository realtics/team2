using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JingongchamAttackBox : MonoBehaviour
{
    private float _aliveTime = 1.0f;
    private AttackInfoSender _sender;

    public float Forward { get { return transform.localScale.x > 0.0f ? 1.0f : -1.0f; } }
    // Start is called before the first frame update
    void Start()
    {
        _sender.Attacker = FindObjectOfType<PlayerCharacter>().transform;
        _sender.Damage = 50.0f;
        _sender.HorizontalExtraMoveDuration = 0.1f;
        _sender.HorizontalExtraMoveValue = -10.0f;
        _sender.StunDuration = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        DeadEffect();
        MoveToForward();
    }

    private void DeadEffect()
    {
        _aliveTime -= Time.deltaTime;

        if (_aliveTime <= 0.0f)
        {
            gameObject.SetActive(false);
            _aliveTime = 1.0f;
        }
    }

    private void MoveToForward()
    {
        transform.Translate(Vector3.right * Forward * 10.0f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Monster"))
            return;

        other.transform.root.GetComponent<Monster>().OnHit(_sender);
    }
}
