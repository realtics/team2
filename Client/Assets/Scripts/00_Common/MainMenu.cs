using System;
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
		if (NetworkManager.Instance.IsConnect)
			NetworkManager.Instance.DisconnectServer();
		// scene Town
		SceneManager.LoadScene((int)SceneIndex.LobbySingle);
		//JS add
		SceneManager.LoadScene((int)SceneIndex.Inventory,LoadSceneMode.Additive);
	}

	private void CheckLogin()
	{
		if (!NetworkManager.Instance.Login)
			return;

		GameStart();
	}

}
