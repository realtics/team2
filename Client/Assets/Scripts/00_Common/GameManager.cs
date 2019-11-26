﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
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
    private int _coin;

    private bool _playerChooseResult = false;

    private GameState _playerState;

    public void MoveToScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
