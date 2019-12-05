using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void GameStart()
	{
		// scene Town
		SceneManager.LoadScene((int)SceneIndex.Lobby);
	}

	public void GameStartSingle()
	{
		// scene Town
		SceneManager.LoadScene((int)SceneIndex.LobbySingle);
	}
}
