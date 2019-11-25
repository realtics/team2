using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectEvent : MonoBehaviour
{
    //public GameObject effect;
    private BaseMonster _monster;
    [SerializeField]
    private Transform _smashEffectPivot;
    [SerializeField]
    private Transform _smashEffect;

    private void Start()
    {
        _monster = transform.root.GetComponent<BaseMonster>();
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
