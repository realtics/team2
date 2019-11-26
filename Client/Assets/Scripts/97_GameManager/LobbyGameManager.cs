﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGameManager : GameManager
{
    private static LobbyGameManager _instance;
    public static LobbyGameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Start()
    {
        _instance = this;
    }
}
