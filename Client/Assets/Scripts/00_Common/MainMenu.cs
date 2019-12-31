using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject signupPanel;

	private void Update()
	{
		CheckLogin();
        OffSginupPanel();
    }
	public void GameStart()
	{
		// scene Town
		SceneManager.LoadScene((int)SceneIndex.Lobby);
        //JS add
    }

	public void GameStartSingle()
	{
		if (NetworkManager.Instance.IsConnect)
			NetworkManager.Instance.DisconnectServer();
		// scene Town
		SceneManager.LoadScene((int)SceneIndex.LobbySingle);
		//JS add
		SceneManager.LoadScene((int)SceneIndex.Inventory,LoadSceneMode.Additive);
		NetworkManager.Instance.IsSingle = true;
	}

	private void CheckLogin()
	{
		if (!NetworkManager.Instance.Login)
			return;

		GameStart();
	}

    private void OffSginupPanel()
    {
        if (!NetworkManager.Instance.SuccessSignup)
            return;

        signupPanel.SetActive(false);
		NetworkManager.Instance.SuccessSignup = false;
    }
}
