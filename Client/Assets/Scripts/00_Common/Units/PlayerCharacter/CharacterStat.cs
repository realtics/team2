using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    private float _maxHp = 1000.0f;
    [SerializeField]
    private float _hp;
    private float _maxMp = 1000.0f;
    [SerializeField]
    private float _mp;
    private float _attackDamage = 10.0f;
    private float _defense = 1.0f;
    private float _attackSpeed = 100.0f;

    // properties
    public float AttackDamage { get { return _attackDamage; } }

    void Start()
    {
        _hp = _maxHp;
        _mp = _maxMp;
    }

    void Update()
    {
        
    }

    public void OnHitDamage(float damage)
    {
        _hp = Mathf.Max(_hp - CalcReceiveDamage(damage), 0);
    }

    private float CalcReceiveDamage(float damage)
    {
        float trueDamage;
        trueDamage = Mathf.Max(damage - _defense, 1);
        return trueDamage;
    }
}
