using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject signupPanel;
    private bool _successSignup;

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
	}

	private void CheckLogin()
	{
		if (!NetworkManager.Instance.Login)
			return;

		GameStart();
	}

    public void SuccessSignup()
    {
        _successSignup = true;
    }

    private void OffSginupPanel()
    {
        if (!_successSignup)
            return;

        signupPanel.SetActive(false);
        _successSignup = false;
    }
}
