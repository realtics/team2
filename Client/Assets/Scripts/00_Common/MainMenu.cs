using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private void Update()
	{
		CheckLogin();
	}
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

	private void CheckLogin()
	{
		if (!NetworkManager.Instance.Login)
			return;

		GameStart();
	}
}
