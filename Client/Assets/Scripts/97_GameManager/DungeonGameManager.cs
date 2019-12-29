using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Dungeon,
    Die,
    Result
}

public class DungeonGameManager : MonoBehaviour
{
    private GameObject _player;

    [SerializeField]
    protected Cinemachine.CinemachineConfiner _cinemachine;

    [SerializeField]
    private Image _fadeOut;

    public float FadeTime = 3f; // Fade효과 재생시간

    private const float _start = 1.0f;
    private const float _end = 0.0f;

    private float _time = 0f;

    private int _countDown;
    private const int _maxDieCountDown = 10;
    private const int _maxResultCountDown = 4;
    private bool _countOver;

    private const float DelayResultSingle = 2.0f;
    private const float DelayResultMulti = 0.5f;
    private const float DelayClear = 2.0f;
    private const float DelayDie = 1.0f;

    private bool _playerChooseResult = false;

    private Coroutine _CoroutineDieCountdown;
    private Coroutine _CoroutineResultCountdown;

    [SerializeField]
    protected GameState _playerState;

    [SerializeField]
    protected int _coin;

    private bool _RES_DUNGEON_CLEAR_RESULT_ITEM = false;
    public bool RES_DUNGEON_CLEAR_RESULT_ITEM { get { return _RES_DUNGEON_CLEAR_RESULT_ITEM; } set { _RES_DUNGEON_CLEAR_RESULT_ITEM = value; } }

    private static DungeonGameManager _instance;
    public static DungeonGameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _instance = this;
        FadeOut();
        PlayerCharacter pc = GameObject.FindObjectOfType<PlayerCharacter>();

        if (pc != null)
            _player = pc.gameObject;

        DNFSceneManager.Instance.Loader();
    }

    // Update is called once per frame
    void Update()
    {
        if (_countOver && _playerState == GameState.Die)
        {
            Invoke(nameof(CountOver), DelayDie);
        }
        if (_countOver && _playerState == GameState.Dungeon)
        {
            Invoke(nameof(GameClear), DelayClear);
        }

        if (_RES_DUNGEON_CLEAR_RESULT_ITEM)
        {
            Invoke(nameof(GameResult), DelayResultMulti);
            _RES_DUNGEON_CLEAR_RESULT_ITEM = false;
        }
    }
    public void FindCameraCollider()
    {
        _cinemachine.InvalidatePathCache();
        _cinemachine.m_BoundingShape2D = GameObject.FindGameObjectWithTag("CameraCollider").GetComponent<Collider2D>();
    }

    public void MoveToScene(int Scene)
    {
        LoadScene(Scene);
    }

    private void LoadScene(int Scene)
    {
        DNFSceneManager.Instance.LoadScene(Scene);
    }

    public void MoveToPlayer(Vector3 position)
    {
        if (_player == null)
            return;

        _player.transform.position = position;
    }

    void CountOver()
    {
        if(NetworkManager.Instance.IsSingle)
        {
            MoveToScene((int)SceneIndex.LobbySingle);
        }
        else
        {
            MoveToScene((int)SceneIndex.Lobby);
        }
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
		if(NetworkManager.Instance.IsSingle)
		{
            Invoke(nameof(GameResult), DelayResultSingle);
		}
        else
        {
            NetworkManager.Instance.DungeonClearResultItem();
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
