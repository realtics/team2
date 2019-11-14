using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectEvent : MonoBehaviour
{
    public GameObject effect;
    private Transform _smashEffectPivot;

    private void Start()
    {
        _smashEffectPivot = transform.parent.GetChild(1).transform;
    }
    public void SmashEffect()
    {
        Vector3 SmashEffectPos = _smashEffectPivot.position;

        FlipEffect();
        Instantiate(effect, SmashEffectPos, Quaternion.Euler(Vector3.zero));
    }
   
    void FlipEffect()
    {
        if (transform.parent.localScale.x > 0)
            effect.GetComponent<SpriteRenderer>().flipX = false;

        else
            effect.GetComponent<SpriteRenderer>().flipX = true;
    }

}
