﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GameStart()
    {
        // scene Town
        SceneManager.LoadScene((int)SceneIndex.Dungen);
    }
}
