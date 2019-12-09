using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	private static PlayerManager _instance;
	public static PlayerManager Instance
	{
		get { return _instance; }
	}

	private PlayerCharacter _playerCharacter;
	public PlayerCharacter PlayerCharacter { get { return _playerCharacter; } set { _playerCharacter = value; } }

	private void Awake()
	{
		_instance = this;
	}

    private void Update()
    {
        
    }
}
