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

    void Awake()
    {
        _playerInfo = GameObject.Find("PlayerInfo").GetComponent<UIPlayerInfo>();
        _monsterInfo = GameObject.Find("MonsterInfo").GetComponent<UIMosterInfo>();

        _gameOver = GameObject.Find("GameOver").GetComponent<UIGameOver>();
    }

    // Use this for initialization
    void Start()
    {
        _instance = this;

        _monsterInfo.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // Stat Test.
        if(Input.GetKeyDown(KeyCode.Z))
        {
            AddPlayerHp(-10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _gameOver.UseCoin();
            AddPlayerMp(-10);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddMonsterHp(-10);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetMonster(null);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetGameOver(true);
        }
    }
    public void SetGameOver(bool isActive)
    {
        _gameOver.gameObject.SetActive(isActive);
    }

    public void AddPlayerHp(float value)
    {
        _playerInfo.AddHp(value);
    }
    public void AddPlayerMp(float value)
    {
        _playerInfo.AddMp(value);
    }
    public void AddMonsterHp(float value)
    {
        _monsterInfo.AddHp(value);
    }
    // Todo.
    public void SetMonster(Monster monster)
    {
        _monsterInfo.gameObject.SetActive(true);
        //임시값.

        //FIXME :  현재체력, 최대체력 구분
        MonsterInfo monsterInfo;
        monsterInfo.name = monster.monsterName;
        monsterInfo.level = monster.monsterLevel;
        monsterInfo.currentHp = monster.currentHp;

        _monsterInfo.SetMonster(monsterInfo);
    }
}
