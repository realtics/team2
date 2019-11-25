using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JingongchamAttackBox : MonoBehaviour
{
    private float _aliveTime = 1.0f;
    private AttackInfoSender _sender;

    public float Forward { get { return transform.localScale.x > 0.0f ? 1.0f : -1.0f; } }
    public AttackInfoSender InfoSender { get { return _sender; } set { _sender = value; } }

    void Start()
    {
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

        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);
    }
}
