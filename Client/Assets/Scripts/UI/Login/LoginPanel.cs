using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
	public InputField idText;
	public InputField passwordText;
	public Button loginButton;


    void Start()
    {
		loginButton.onClick.AddListener(() => { NetworkManager.Instance.CheckBeforeLogin(idText.text, passwordText.text); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
