using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterBaseStat
{
    public int physicalAttack;
    public int magicAttack;
    public int physicalDefense;
    public int magicDefense;
    public int strength;
    public int intelligence;
    public int health;
    public int mentality;
    public int hangma;
}

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

    [SerializeField] private CharacterBaseStat _baseStat;
    [SerializeField] private CharacterBaseStat _totalStat;

    // properties
    public float AttackDamage { get { return _attackDamage; } set {_attackDamage = value; } }
    public float MaxHp { get { return _maxHp; } }
    public float Hp { get { return _hp; } }
    public float MaxMp { get { return _maxMp; } }
    public float Mp { get { return _mp; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public BaseUnit Unit { get { return _unit; } }
    public CharacterBaseStat TotalStat { get { return _totalStat; } }
	public bool IsDie { get { return _hp <= 0.0f ? true : false; } }
	public bool IsSingle { get { return _isSingle; } set { _isSingle = value; } }

    private void Awake()
    {
        _uiPlayerInfo = FindObjectOfType<UIPlayerInfo>();
		_isSingle = true;

	}

    private void Start()
    {
        
        //_hp = _maxHp;
        //_mp = _maxMp;
        //_uiPlayerInfo = FindObjectOfType<UIPlayerInfo>();
        //_uiPlayerInfo.SetPlayerHp(_hp, _maxHp);
        //_uiPlayerInfo.SetPlayerMp(_mp, _maxMp);
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
        _unit.SetDie();
        DungeonGameManager.Instance.GameOver();
    }

    public void SetUnit(BaseUnit unit)
    {
        _unit = unit;
    }

    public void RefreshExtraStat()
    {
        _totalStat.hangma = _baseStat.hangma + PlayerManager.Instance.EquipmentStat.hangma;
        _totalStat.health = _baseStat.health + PlayerManager.Instance.EquipmentStat.health;
        _totalStat.intelligence = _baseStat.intelligence + PlayerManager.Instance.EquipmentStat.intelligence;
        _totalStat.magicAttack = _baseStat.magicAttack + PlayerManager.Instance.EquipmentStat.magicAttack;
        _totalStat.magicDefense = _baseStat.magicDefense + PlayerManager.Instance.EquipmentStat.magicDefense;
        _totalStat.mentality = _baseStat.mentality + PlayerManager.Instance.EquipmentStat.mentality;
        _totalStat.physicalAttack = _baseStat.physicalAttack + PlayerManager.Instance.EquipmentStat.physicalAttack;
        _totalStat.physicalDefense = _baseStat.physicalDefense + PlayerManager.Instance.EquipmentStat.physicalDefense;
        _totalStat.strength = _baseStat.strength + PlayerManager.Instance.EquipmentStat.strength;

        _attackDamage = _totalStat.physicalAttack * _totalStat.strength / 100;
        _maxHp = _totalStat.health * 10;
        _maxMp = _totalStat.mentality * 10;
        _hp = _maxHp;
        _mp = _maxMp;
       
        _uiPlayerInfo.SetPlayerHp(_hp, _maxHp);
        _uiPlayerInfo.SetPlayerMp(_mp, _maxMp);
    }

	public void Revive()
	{
		if (!IsDie)
			return;

		_hp = _maxHp;
		_mp = _maxMp;
		_uiPlayerInfo.SetPlayerHp(_hp, _maxHp);
	}
}
