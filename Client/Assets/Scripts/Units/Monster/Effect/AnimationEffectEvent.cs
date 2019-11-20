using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectEvent : MonoBehaviour
{
    //public GameObject effect;
    private Monster _monster;

    private Transform _smashEffectPivot;
    private Transform _smashEffect;

    private void Start()
    {
        _monster = transform.root.GetComponent<Monster>();

        _smashEffectPivot = transform.root.Find("SmashEffectPivot");
        _smashEffect = _smashEffectPivot.GetChild(0).gameObject.transform;
    }
    private void OnSmashEffect()
    {
        if (_monster.IsAttack)
        {
            Vector3 SmashEffectPos = _smashEffectPivot.position;

            _smashEffect.gameObject.SetActive(true);
        }
    }

    public void OnSmashAttackBox()
    {
        if (_monster.IsAttack)
        {
            _monster.ActiveSmashHitBox();
        }
    }

    public void OffSmashAttackBox()
    {
        _monster.InactiveSmashHitBox();
    }
}
