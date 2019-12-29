using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectEvent : MonoBehaviour
{
    //public GameObject effect;
    private BaseMonster _monster;
    //private Tauarmy _tauarmy;
   
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
}
