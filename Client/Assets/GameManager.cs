using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum PlayerState
{ 
    Alive,
    Die
}
enum SceneIndex
{
    MainMenu,
    Dungen
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _coin;

    private int _countDown;
    private const int _maxCountDown = 10;

    private CharacterStat _playerStat;

    private PlayerState _playerState;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public CharacterMovement player
    {
        get
        {
            return player;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerState = PlayerState.Alive;
        _instance = this;
        _playerStat = FindObjectOfType<CharacterStat>().GetComponent<CharacterStat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //To do.
    // GameManager에 코인 사용, 코인을 사용할 수 있는 Countdown과 코인을 전부 사용 후 menu 화면으로 넘어가기.
    public void GameOver()
    {
        if (_playerState == PlayerState.Alive)
        {
            _playerState = PlayerState.Die;
            UIHelper.Instance.SetGameOver(true, _coin);
            _countDown = _maxCountDown;
            UIHelper.Instance.SetTime(_countDown);
            StartCoroutine(SecondCountdown());
        }
    }
    public void UseCoin()
    {
        if(_coin > 0)
        {
            _coin -= 1;
            _countDown = _maxCountDown;
            StopCoroutine(SecondCountdown());
        }
    }

    public void CountOver()
    {
        _countDown = _maxCountDown;
        _playerState = PlayerState.Alive;
        StopCoroutine(SecondCountdown());

        MoveToScene((int)SceneIndex.MainMenu);
    }

    public void MoveToScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    IEnumerator SecondCountdown()
    {
        while (true)
        {
            if (_countDown > 0)
            {
                _countDown -= 1;
                UIHelper.Instance.SetTime(_countDown);
            }
            else
            {
                CountOver();
                UIHelper.Instance.SetGameOver(false, _coin);
            }
            yield return new WaitForSeconds(1);
        }
    }
}
