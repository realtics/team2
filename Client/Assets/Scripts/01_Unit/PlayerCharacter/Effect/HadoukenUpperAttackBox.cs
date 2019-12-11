using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenUpperAttackBox : MonoBehaviour
{
    public GameObject attackEffectPrefab;
    private HadoukenAttackBox _attackBox;
    private AttackInfoSender _sender;
    private BaseUnit _unit;

    public AttackInfoSender InfoSender { get { return _sender; } set { _sender = value; } }
    public BaseUnit Unit { get { return _unit; } set { _unit = value; } }

    public void ClipEvent_DeadEffect()
    {
        gameObject.SetActive(false);
    }

    public void ClipEvent_SpawnHadoken()
    {
        GameObject instanceEffect = ObjectPoolManager.Instance.GetRestObject(attackEffectPrefab);
        _attackBox = instanceEffect.GetComponent<HadoukenAttackBox>();

        Vector3 pos = attackEffectPrefab.transform.position;
        pos.x *= _unit.Forward;

        instanceEffect.transform.position = _sender.Attacker.transform.position + pos;

        Vector3 flipScale = attackEffectPrefab.transform.localScale;
        flipScale.x *= _unit.Forward;
        instanceEffect.transform.localScale = flipScale;

        _attackBox.InfoSender = _sender;
    }
}
