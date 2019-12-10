using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.IO;
using System.Security.Cryptography;


public class Login : MonoBehaviour
{
	public Text _id;
	public InputField _password;
	public Text _name;

	// 키로 사용하기 위한 암호 정의
	private static readonly string PASSWORD = "3ds1s334e4dcc7c4yz4554e732983h";

	// 인증키 정의
	private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

	// 암호화
	public static string AESEncrypt128(string plain)
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

	// 복호화
	public static string AESDecrypt128(string encrypt)
	{
		byte[] encryptBytes = Convert.FromBase64String(encrypt);

		RijndaelManaged myRijndael = new RijndaelManaged();
		myRijndael.Mode = CipherMode.CBC;
		myRijndael.Padding = PaddingMode.PKCS7;
		myRijndael.KeySize = 128;

		MemoryStream memoryStream = new MemoryStream(encryptBytes);

		ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

		CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

		byte[] plainBytes = new byte[encryptBytes.Length];

		int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

		string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

		cryptoStream.Close();
		memoryStream.Close();

		return plainString;
	}

	void Start()
	{
		string str = "원본 문자 정보";
		Debug.Log("plain : " + str1);

		string str1 = AESEncrypt128(str1);
		Debug.Log("AES128 encrypted : " + str1);

		string str2 = AESDecrypt128(str2);
		Debug.Log("AES128 decrypted : " + str2);
	}


	void Update()
	{
		//if (_id.text.Contains("#")) { } //특정 문자 찾기

		Debug.Log(_id.text);
		Debug.Log(_password.text);
		Debug.Log(_name.text);
	}


}