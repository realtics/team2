using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{ 
    Dungeon,
    Die,
    Result
}
public enum SceneIndex
{
    MainMenu,
    Lobby,
    Dungen
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    protected int _coin;

    [SerializeField]
    private Image _fadeOut;

    [SerializeField]
    protected Cinemachine.CinemachineConfiner _cinemachine;

    public float FadeTime = 3f; // Fade효과 재생시간

    private const float _start = 1.0f;
    private const float _end = 0.0f;

    private float _time = 0f;

    private bool _isFadeOutPlaying = false;

    protected GameObject _player;

    protected virtual void Start()
    {
        FadeOut();
        _player = GameObject.FindObjectOfType<PlayerCharacter>().gameObject;
    }

    public void MoveToScene(int Scene)
    {
        LoadScene(Scene);
    }

    private void LoadScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void MoveToPlayer(Vector3 position)
    {
        _player.transform.position = position;
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
        _isFadeOutPlaying = true;
        Color fadecolor = _fadeOut.color;
        _time = 0f;

        while (fadecolor.a > 0f)
        {
            _time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(_start, _end, _time);
            _fadeOut.color = fadecolor;
            yield return null;
        }

        _isFadeOutPlaying = false;
    }

}
