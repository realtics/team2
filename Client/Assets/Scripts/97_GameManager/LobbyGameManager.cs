using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyGameManager : MonoBehaviour
{
    private static LobbyGameManager _instance;
    public static LobbyGameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    [SerializeField]
    private Image _fadeOut;

    public float FadeTime = 3f; // Fade효과 재생시간

    private const float _start = 1.0f;
    private const float _end = 0.0f;

    private float _time = 0f;

    private void Start()
    {
        _instance = this;
        FadeOut();
		if (NetworkManager.Instance.IsConnect) 
		{
			NetworkManager.Instance.LoginToTown();
		}
    }
    public void FadeOut()
    {
        FadeIn();
        StartCoroutine(Fadeoutplay());
    }
    private void FadeIn()
    {
        Color fadecolor = _fadeOut.color;
        fadecolor.a = 1.0f;
        _fadeOut.color = fadecolor;
    }

    IEnumerator Fadeoutplay()
    {
        Color fadecolor = _fadeOut.color;
        _time = 0f;

        if (SceneManager.GetSceneByBuildIndex((int)SceneIndex.Inventory).isLoaded)
        {
            SceneManager.UnloadSceneAsync((int)SceneIndex.Inventory);
        }
        while (fadecolor.a > 0f)
        {
            _time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(_start, _end, _time);
            _fadeOut.color = fadecolor;
            yield return null;
        }
    }
}
