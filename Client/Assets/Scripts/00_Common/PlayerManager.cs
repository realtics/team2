using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
	private PlayerManager() { }
	private static PlayerManager _instance;
	public static PlayerManager Instance
	{
		get 
		{
			if (_instance == null)
				_instance = new PlayerManager();

			return _instance; 
		}
	}

	private string _nickName;
    private CharacterBaseStat _equipmentStat;

    public string NickName { get { return _nickName; } set { _nickName = value; } }
	private PlayerCharacter _playerCharacter;
	public PlayerCharacter PlayerCharacter { get { return _playerCharacter; } set { _playerCharacter = value; } }
	public CharacterStat Stat { get { return _playerCharacter.Movement.Stat; } }
    public CharacterBaseStat EquipmentStat { get { return _equipmentStat; }}

    public void SetEquipmentStat(CharacterBaseStat stat)
    {
        _equipmentStat = stat;
        Stat.RefreshExtraStat();
    }
}
