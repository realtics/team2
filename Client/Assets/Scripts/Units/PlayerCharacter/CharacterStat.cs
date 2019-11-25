using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    private BaseUnit _unit;
    private float _maxHp = 1000.0f;
    [SerializeField]
    private float _hp;
    private float _maxMp = 1000.0f;
    [SerializeField]
    private float _mp;
    [SerializeField]
    private float _attackDamage = 100.0f;
    private float _defense = 1.0f;
    [SerializeField]
    private float _attackSpeed = 100.0f;
    private UIPlayerInfo _uiPlayerInfo;

    // properties
    public float AttackDamage { get { return _attackDamage; } }
    public float MaxHp { get { return _maxHp; } }
    public float Hp { get { return _hp; } }
    public float MaxMp { get { return _maxMp; } }
    public float Mp { get { return _mp; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public BaseUnit Unit { get { return _unit; } }

    void Start()
    {
        _hp = _maxHp;
        _mp = _maxMp;
        _uiPlayerInfo = FindObjectOfType<UIPlayerInfo>();
        _uiPlayerInfo.SetPlayerHp(_hp, _maxHp);
        _uiPlayerInfo.SetPlayerMp(_mp, _maxMp);
    }

    void Update()
    {
        
    }

    public void OnHitDamage(AttackInfoSender sender)
    {
        float damage = CalcReceiveDamage(sender.Damage);
        _hp = Mathf.Max(_hp - damage, 0);

        if (_hp == 0.0f)
            SetDie();

        _uiPlayerInfo.SetPlayerHp(_hp, _maxHp);
    }

    private float CalcReceiveDamage(float damage)
    {
        float trueDamage;
        trueDamage = Mathf.Max(damage - _defense, 1);
        return trueDamage;
    }

    private void SetDie()
    {
        DungeonGameManager.Instance.GameOver();
    }

    public void SetUnit(BaseUnit unit)
    {
        _unit = unit;
    }
}
