﻿using UnityEngine;
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
    [SerializeField]
    private UIPlayerInfo _playerInfo;
    [SerializeField]
    private UIMosterInfo _monsterInfo;
    [SerializeField]
    private UIGameOver _gameOver;
    [SerializeField]
    private UIGameResult _gameResult;
    [SerializeField]
    private UIDungeonClearMenu _dungeonClearMenu;

    // Use this for initialization
    void Start()
    {
        _instance = this;

        if(_monsterInfo != null)
            _monsterInfo.gameObject.SetActive(false);
        if (_gameOver != null)
            _gameOver.gameObject.SetActive(false);
        if (_gameResult != null)
            _gameResult.gameObject.SetActive(false);
        if (_dungeonClearMenu != null)
            _dungeonClearMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
    public void SetDungeonClearMenu(bool isActive)
    {
        _dungeonClearMenu.gameObject.SetActive(isActive);
    }
    public void GameResultSetTime(int time)
    {
        _gameResult.SetTime(time);
    }
    public void OpenResultBox(int index)
    {
        _gameResult.OpenResultBox(index);
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
        monsterInfo.name = monster.MonsterName;
        monsterInfo.level = monster.MonsterLevel;
        monsterInfo.currentHp = monster.CurrentHp;
        monsterInfo.maxHp = monster.MaxHp;
        monsterInfo.index = monster.MonsterType;
        _monsterInfo.SetMonster(monsterInfo);

    }
}
