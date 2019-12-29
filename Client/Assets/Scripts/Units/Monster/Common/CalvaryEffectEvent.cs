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
    [Space]
    [SerializeField]
    private Transform _smash01Effect;
    [SerializeField]
    private Transform _smash02Effect;
    [Space]
    [SerializeField]
    private Transform[] _splash01Effect;
    [SerializeField]
    private Transform _upSplash02Effect;
    [SerializeField]
    private Transform _downSplash02Effect;

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

    private void OnSmash01Effect()
    {
        if (_monster.IsAttack)
        {
            _smash01Effect.gameObject.SetActive(true);
        }
    }

    private void OnSmash02Effect()
    {
        if (_monster.IsAttack)
        {
            _smash02Effect.gameObject.SetActive(true);
        }
    }

    private void OnSplash01Effect(int index)
    {
        if (_monster.IsAttack)
        {
            _splash01Effect[index].gameObject.SetActive(true);
        }
    }

    private void OnSplash02Effect()
    {
        if (_monster.IsAttack)
        {
            _upSplash02Effect.gameObject.SetActive(true);
            _downSplash02Effect.gameObject.SetActive(true);
        }
    }
}

