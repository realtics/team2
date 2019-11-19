using UnityEngine;
using System.Collections;

public class UIHelper : MonoBehaviour
{
    private static UIHelper _instance;
    public static UIHelper Instance
    {
        get
        {
            return _instance;       
        }
    }

    private UIPlayerInfo _playerInfo;
    private UIMosterInfo _monsterInfo;

    private UIGameOver _gameOver;
    private UIGameResult _gameResult;

    void Awake()
    {
        _playerInfo = GameObject.Find("PlayerInfo").GetComponent<UIPlayerInfo>();
        _monsterInfo = GameObject.Find("MonsterInfo").GetComponent<UIMosterInfo>();

        _gameOver = GameObject.Find("GameOver").GetComponent<UIGameOver>();
        _gameResult = GameObject.Find("GameResult").GetComponent<UIGameResult>();
    }

    // Use this for initialization
    void Start()
    {
        _instance = this;

        _monsterInfo.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(false);
        _gameResult.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // hack Test.
        if(Input.GetKeyDown(KeyCode.V))
        {
            GameManager.Instance.GameResult();
        }
    }
    public void SetGameOver(bool isActive)
    {
        _gameOver.gameObject.SetActive(isActive);
    }
    public void SetGameOver(bool isActive, int coin)
    {
        _gameOver.gameObject.SetActive(isActive);
        _gameOver.Coin = coin;
    }
    public void GameOverSetTime(int time)
    {
        _gameOver.SetTime(time);
    }

    public void SetGameResult(bool isActive)
    {
        _gameResult.gameObject.SetActive(isActive);
    }
    public void GameResultSetTime(int time)
    {
        _gameResult.SetTime(time);
    }
    public void SetMonsterHp(float CurrentHp, float MaxHp)
    {
        _monsterInfo.SetHp(CurrentHp, MaxHp);
    }
    // Todo.
    public void SetMonster(Monster monster)
    {
        _monsterInfo.gameObject.SetActive(true);
        
        //FIXME :  현재체력, 최대체력 구분
        MonsterInfo monsterInfo;
        monsterInfo.name = monster.monsterName;
        monsterInfo.level = monster.monsterLevel;
        monsterInfo.currentHp = monster.currentHp;

        _monsterInfo.SetMonster(monsterInfo);
    }
}
