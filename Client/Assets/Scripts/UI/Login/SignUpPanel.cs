using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
	public InputField id;
	public InputField password;
	public InputField passwordCheck;
	public InputField nickName;

    void Start()
    {
	}

    void Update()
    {
    }

	public void SignUpButton()
	{
		if (id.text == "")
		{
			ToastMessagePanel.Instance.SetToastMessage("아이디를 입력해 주세요.");
			return;
		}

		if (nickName.text == "")
		{
			ToastMessagePanel.Instance.SetToastMessage("닉네임을 입력해주세요.");
			return;
		}

		if (password.text != passwordCheck.text)
		{
			ToastMessagePanel.Instance.SetToastMessage("비밀번호 확인이 다릅니다.");
			return;
		}
	}
}
