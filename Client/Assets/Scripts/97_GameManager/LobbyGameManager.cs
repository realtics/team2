using System.Collections;
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

    protected override void Start()
    {
        base.Start();
        _instance = this;

		NetworkManager.Instance.LoginToTown();
    }
}
