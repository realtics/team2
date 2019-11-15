using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectEvent : MonoBehaviour
{
    public GameObject effect;
    private Monster _monster;

    private Transform _smashEffectPivot;

    private void Start()
    {
        _monster = transform.GetComponentInParent<Monster>();

        _smashEffectPivot = transform.parent.Find("SmashEffectPivot"); 
    }
    private void OnSmashEffect()
    {
        Vector3 SmashEffectPos = _smashEffectPivot.position;

        FlipEffect();
        Instantiate(effect, SmashEffectPos, Quaternion.Euler(Vector3.zero));

        _monster.ActiveSmashHitBox();
    }
    
    private void FlipEffect()
    {
        if (transform.parent.localScale.x > 0)
            effect.GetComponent<SpriteRenderer>().flipX = false;

        else
            effect.GetComponent<SpriteRenderer>().flipX = true;
    }
}
