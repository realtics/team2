using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
	public InputField idText;
	public InputField passwordText;

	// 키로 사용하기 위한 암호 정의
	private static readonly string PASSWORD = "9g3gbh6zsdfqioq1zdnsgff2nljkc4";

	// 인증키 정의
	private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

	// 암호화
	private static string AESEncrypt128(string plain)
	{
		byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

		RijndaelManaged myRijndael = new RijndaelManaged();
		myRijndael.Mode = CipherMode.CBC;
		myRijndael.Padding = PaddingMode.PKCS7;
		myRijndael.KeySize = 128;

		MemoryStream memoryStream = new MemoryStream();

		ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

		CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
		cryptoStream.Write(plainBytes, 0, plainBytes.Length);
		cryptoStream.FlushFinalBlock();

		byte[] encryptBytes = memoryStream.ToArray();

		string encryptString = Convert.ToBase64String(encryptBytes);

		cryptoStream.Close();
		memoryStream.Close();

		return encryptString;
	}

	void Start()
    {
		//loginButton.onClick.AddListener(() => { NetworkManager.Instance.CheckBeforeLogin(idText.text, passwordText.text); });
    }

	public void ClickLoginButton()
	{
		NetworkManager.Instance.ConnectToServer();
		string password = AESEncrypt128(passwordText.text);

		NetworkManager.Instance.CheckBeforeLogin(idText.text, password);
	}
}
