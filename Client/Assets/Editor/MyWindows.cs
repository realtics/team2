using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MyWindows : MonoBehaviour
{
    static JsonManagement _jsonManagement = new JsonManagement();
    // Add a menu item named "Do Something" to MyMenu in the menu bar.

    [MenuItem("MapTool/AddDungeon")]
    static void Add()
    {
        _jsonManagement.AddDungeon();
    }
    [MenuItem("MapTool/SetBossDungeon")]
    static void SetDungeonBoss()
    {
        _jsonManagement.SetDungeonBoss();
    }
    [MenuItem("MapTool/ClearDungeonData")]
    static void Clear()
    {
        _jsonManagement.JsonClear();
    }
    [MenuItem("MapTool/Save")]
    static void Save()
    {
        _jsonManagement.JsonSave();
    }
    [MenuItem("MapTool/Load")]
    static void Load()
    {
        var jsonData = _jsonManagement.JsonLoad<DungeonJsonData>("Test2");
    }

}
