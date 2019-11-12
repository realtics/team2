using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MyWindows : MonoBehaviour
{
    static JsonManagement _jsonManagement = new JsonManagement();
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("MapTool/Save")]
    static void Save()
    {
        _jsonManagement.JsonSave();
    }
    [MenuItem("MapTool/Load")]
    static void Load()
    {
        Debug.Log("Doing Something...");
    }
    [MenuItem("MapTool/Custom Object")]
    static void InstObject()
    {
        _jsonManagement.InstObject();
    }
}
