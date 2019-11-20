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
    private const int _maxDieCountDown = 10;
    private const int _maxResultCountDown = 4;
    private bool _countOver;
    private float _currentTime = 0.0f;

    private bool _playerChooseResult = false;

    private GameObject _dungeonClearMenu;

    private PlayerState _playerState;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _dungeonClearMenu = GameObject.Find("DungeonClearMenu");
        _dungeonClearMenu.SetActive(false);

        _playerState = PlayerState.Alive;
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(_countOver && _playerState == PlayerState.Die)
        {
            Invoke("CountOver", 1f);
        }
        if (_countOver && _playerState == PlayerState.Alive)
        {
            if(!_dungeonClearMenu.activeSelf)
                _dungeonClearMenu.SetActive(true);
        }
    }

    void CountOver()
    {
        MoveToScene((int)SceneIndex.MainMenu);
    }

    public void GameOver()
    {
        if (_playerState == PlayerState.Alive)
        {
            _playerState = PlayerState.Die;
            UIHelper.Instance.SetGameOver(true, _coin);
            _countDown = _maxDieCountDown;
            UIHelper.Instance.GameOverSetTime(_countDown);
            StartCoroutine(DieSecondCountdown());
        }
    }
    public void UseCoin()
    {
        if(_coin > 0)
        {
            _coin -= 1;
            _countDown = _maxDieCountDown;
            StopCoroutine(DieSecondCountdown());
        }
    }

    public void DieCountOver()
    {
        _countDown = _maxDieCountDown;
        _playerState = PlayerState.Alive;
    }

    public void GameResult()
    {
        UIHelper.Instance.SetGameResult(true);
        _countDown = _maxResultCountDown;
        UIHelper.Instance.GameResultSetTime(_countDown);
        StartCoroutine(ResultSecondCountdown());
    }
    public void OnClickResultBox(int index)
    {
        if(!_playerChooseResult)
        {
            UIHelper.Instance.OpenResultBox(index);
            _playerChooseResult = true;
        }
    }

    public void MoveToScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    IEnumerator DieSecondCountdown()
    {
        while (true)
        {
            if (_countDown > 0)
            {
                _countDown -= 1;
                UIHelper.Instance.GameOverSetTime(_countDown);
            }
            else
            {
                UIHelper.Instance.SetGameOver(false, _coin);
                _countOver = true;

                StopCoroutine(DieSecondCountdown());
            }
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator ResultSecondCountdown()
    {
        while (true)
        {
            if (_countDown > 0)
            {
                _countDown -= 1;
                UIHelper.Instance.GameResultSetTime(_countDown);
            }
            else
            {
                UIHelper.Instance.SetGameResult(false);
                _countOver = true;
                StopCoroutine(ResultSecondCountdown());
                
            }
            yield return new WaitForSeconds(1);
        }
    }

    //TODO : 보스몬스터 죽었을 시 알리기
    public void NoticeGameClear()
    {
        GameResult();
    }
}
