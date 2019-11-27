using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenAttackBox : MonoBehaviour
{
    [SerializeField]
    public float AliveTime = 0.75f;
    private Animator _animator;
    private AttackInfoSender _sender;
    private float _aliveTime;

    public AttackInfoSender InfoSender { get { return _sender; } set { _sender = value; } }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _aliveTime = AliveTime;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void ClipEvent_DeadEffect()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("MonsterHitBox"))
            return;

        other.transform.root.GetComponent<BaseMonster>().OnHit(_sender);
    }
}
