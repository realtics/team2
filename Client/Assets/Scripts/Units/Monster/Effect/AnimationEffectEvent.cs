using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectEvent : MonoBehaviour
{
    //public GameObject effect;
    private BaseMonster _monster;
    [SerializeField]
    private Transform _baseAttackEffectPivot;
    [SerializeField]
    private Transform _baseAttackEffect;

    private void Start()
    {
        _monster = transform.root.GetComponent<BaseMonster>();
    }

    private void OnSmashEffect()
    {
        if (_monster.IsAttack)
        {
            //FIXME : 오브젝트풀링 적용시 사용
            //Vector3 SmashEffectPos = _baseAttackEffectPivot.position;

            _baseAttackEffect.gameObject.SetActive(true);
        }
    }

    public void OnBaseAttackBox()
    {
        if (_monster.IsAttack)
        {
            _monster.ActiveBaseAttackBox();
        }
    }

    public void OffBaseAttackBox()
    {
        _monster.InactiveBaseAttackHitBox();
    }
}
