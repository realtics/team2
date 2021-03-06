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
    [Header("[Common]")]
    [SerializeField]
    private UIPlayerInfo _playerInfo;
    [Header("[Town]")]
    [SerializeField]
    private UIDungeonSelectMenu _dungeonSelectMenu;
    [Header("[Dungeon]")]
    [SerializeField]
    private UIMosterInfo _monsterInfo;
    [SerializeField]
    private UIGameOver _gameOver;
    [SerializeField]
    private UIGameResult _gameResult;
    [SerializeField]
    private UIDungeonClearMenu _dungeonClearMenu;
    [SerializeField]
    private UIMiniMap _miniMap;    
    [SerializeField]
    private UIDungeonTitle _dungeonTitle;

    public UIMiniMap miniMap
    {
        get
        {
            return _miniMap;
        }
    }


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
        if (_dungeonSelectMenu != null)
            _dungeonSelectMenu.gameObject.SetActive(false);
        if (_miniMap != null)
            _miniMap.gameObject.SetActive(true);        
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
    public void SetDungeonSelectMenu(bool isActive)
    {
        _dungeonSelectMenu.gameObject.SetActive(isActive);
    }
    public void GameResultSetTime(int time)
    {
        _gameResult.SetTime(time);
    }
    public void OpenResultBox(int index)
    {
        _gameResult.OpenResultBox(index);
    }    
    public void SetDungeonTitle(string name)
    {
        _dungeonTitle.SetTitle(name);
    }

    // Todo.
    public void SetMonster(BaseMonster monster)
    {
        _monsterInfo.gameObject.SetActive(true);
        
        //FIXME :  현재체력, 최대체력 구분
        UIMonsterInfo monsterInfo;
        monsterInfo.name = monster.MonsterName;
        monsterInfo.level = monster.MonsterLevel;
        monsterInfo.currentHp = monster.CurrentHp;
        monsterInfo.maxHp = monster.MaxHp;
        monsterInfo.index = monster.MonsterType;
        _monsterInfo.SetMonster(monsterInfo);

    }
}
