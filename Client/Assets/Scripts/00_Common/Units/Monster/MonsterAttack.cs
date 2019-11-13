using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = gameObject.GetComponentInChildren<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        AttackStart();
    }

    private void AttackStart()
    {
        gameObject.GetComponent<MonsterMovement>().enabled = false;

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _anim.SetBool("isAttacking", false);
            this.enabled = false;
            gameObject.GetComponent<MonsterMovement>().enabled = true;
        }
    }
}
