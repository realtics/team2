using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{ 
    Dungeon,
    Die,
    Result
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

    private const float _delayResult = 2.0f;
    private const float _delayClear = 2.0f;
    private const float _delayDie = 1.0f;

    private bool _playerChooseResult = false;

    private GameState _playerState;

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
        _playerState = GameState.Dungeon;
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(_countOver && _playerState == GameState.Die)
        {
            Invoke(nameof(CountOver), _delayDie);
        }
        if (_countOver && _playerState == GameState.Dungeon)
        {
            Invoke(nameof(GameClear), _delayClear);
        }
    }

    void CountOver()
    {
        MoveToScene((int)SceneIndex.MainMenu);
    }

    public void GameClear()
    {
        UIHelper.Instance.SetGameResult(false);
        UIHelper.Instance.SetDungeonClearMenu(true);
    }

    public void GameOver()
    {
        if (_playerState == GameState.Dungeon)
        {
            _playerState = GameState.Die;
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
        _playerState = GameState.Dungeon;
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
                _countOver = true;
                if(!_playerChooseResult)
                {
                    OnClickResultBox(0);
                }
                StopCoroutine(ResultSecondCountdown());
                
            }
            yield return new WaitForSeconds(1);
        }
    }

    //TODO : 보스몬스터 죽었을 시 알리기
    public void NoticeGameClear()
    {
        Invoke(nameof(GameResult), _delayResult);
    }
}
