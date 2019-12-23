using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
	const int MinIdLength = 4;
	const int MinPasswordLength = 4;
	const int MinNicknameLength = 2;

	public InputField idInputField;
	public InputField passwordInputField;
	public InputField passwordCheckInputField;
	public InputField nickNameInputField;

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

	public void SignUpButton()
	{
		if (idInputField.text == "")
		{
			ToastMessagePanel.Instance.SetToastMessage("아이디를 입력해 주세요.");
			return;
		}

		if (idInputField.text.Length < MinIdLength)
		{
			ToastMessagePanel.Instance.SetToastMessage($"아이디는 {MinIdLength}자 이상 입력하세요.");
			return;
		}

		if (nickNameInputField.text == "")
		{
			ToastMessagePanel.Instance.SetToastMessage("닉네임을 입력해주세요.");
			return;
		}

		if (nickNameInputField.text.Length < MinNicknameLength)
		{
			ToastMessagePanel.Instance.SetToastMessage($"닉네임은 {MinNicknameLength}자 이상 입력하세요.");
			return;
		}

		if (passwordInputField.text.Length < MinPasswordLength)
		{
			ToastMessagePanel.Instance.SetToastMessage($"비밀번호는 {MinPasswordLength}자 이상 입력하세요.");
			return;
		}

		if (passwordInputField.text != passwordCheckInputField.text)
		{
			ToastMessagePanel.Instance.SetToastMessage("비밀번호 확인이 다릅니다.");
			return;
		}

		string password = AESEncrypt128(passwordInputField.text);
		NetworkManager.Instance.ConnectToServer();
		NetworkManager.Instance.SignUpUser(idInputField.text, password, nickNameInputField.text);
	}
}
