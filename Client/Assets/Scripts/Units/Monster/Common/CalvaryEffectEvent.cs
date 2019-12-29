using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalvaryEffectEvent : MonoBehaviour
{
    private BaseMonster _monster;

    [SerializeField]
    private Transform _smashCircleEffectLeftTop;
    [SerializeField]
    private Transform _smashCircleEffectLeftBottom;
    [SerializeField]
    private Transform _smashCircleEffectRightTop;
    [SerializeField]
    private Transform _smashCircleEffectRightBottom;

    private void Start()
    {
        _monster = transform.root.GetComponent<BaseMonster>();
    }

    private void OnSmashCircleEffect()
    {
        if (_monster.IsAttack)
        {
            _smashCircleEffectLeftTop.gameObject.SetActive(true);
            _smashCircleEffectLeftBottom.gameObject.SetActive(true);
            _smashCircleEffectRightTop.gameObject.SetActive(true);
            _smashCircleEffectRightBottom.gameObject.SetActive(true);
        }
    }
}
