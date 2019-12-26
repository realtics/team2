using UnityEngine;
using System.Collections;

enum PartyNumber
{ 
    p1 = 0,
    p2,
    p3,
    p4
}
public class DungeonGameManager : GameManager
{
    private int _countDown;
    private const int _maxDieCountDown = 10;
    private const int _maxResultCountDown = 4;
    private bool _countOver;

    private const float _delayResult = 2.0f;
    private const float _delayClear = 2.0f;
    private const float _delayDie = 1.0f;

    private bool _playerChooseResult = false;

    private Coroutine _CoroutineDieCountdown;
    private Coroutine _CoroutineResultCountdown;

    [SerializeField]
    protected GameState _playerState;

    private static DungeonGameManager _instance;
    public static DungeonGameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _instance = this;
        DNFSceneManager.instance.Loader();
    }

    // Update is called once per frame
    void Update()
    {
        if (_countOver && _playerState == GameState.Die)
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
        MoveToScene((int)SceneIndex.Lobby);
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
            _CoroutineDieCountdown = StartCoroutine(DieSecondCountdown());
        }
    }
    public void UseCoin()
    {
        if (_coin > 0)
        {
            StopCoroutine(_CoroutineDieCountdown);
            _coin -= 1;
            _countDown = _maxDieCountDown;
            PlayerManager.Instance.PlayerCharacter.Revive();
            UIHelper.Instance.SetGameOver(false, _coin);
            ResetDie();
        }
    }

    public void ResetDie()
    {
        _countDown = _maxDieCountDown;
        _playerState = GameState.Dungeon;
    }

    public void GameResult()
    {
        UIHelper.Instance.SetGameResult(true);
        _countDown = _maxResultCountDown;
        UIHelper.Instance.GameResultSetTime(_countDown);
        _CoroutineResultCountdown = StartCoroutine(ResultSecondCountdown());
    }
    public void OnClickResultBox(int index)
    {
        if (!_playerChooseResult)
        {
            UIHelper.Instance.OpenResultBox(index);
            _playerChooseResult = true;
        }
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

                StopCoroutine(_CoroutineDieCountdown);
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
                if (!_playerChooseResult)
                {
                    OnClickResultBox(0);
                }
                StopCoroutine(_CoroutineResultCountdown);

            }
            yield return new WaitForSeconds(1);
        }
    }

    public void NoticeBossDie()
    {
		if(NetworkManager.Instance.IsConnect)NetworkManager.Instance.DungeonClearResultItem();

        Invoke(nameof(GameResult), _delayResult);
    }
}
