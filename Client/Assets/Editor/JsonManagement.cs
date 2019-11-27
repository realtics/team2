﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;


public class JsonManagement
{
    JsonData jsonData = new JsonData();
    JsonSerializerSettings setting = new JsonSerializerSettings();

    private const string _objectTag = "FieldObject";
    private const string _monsterTag = "Monster";
    private const string _potalSceneTag = "PotalScene";
    private const string _potalTranportTag = "PotalTransport"; 

    public JsonManagement()
    {
        jsonData = new JsonData();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public void JsonSave()
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile(Application.dataPath, "Test2", strJsonData);
    }

    public void JsonClear()
    {
        jsonData.dungeonObjectList.Clear();
    }

    public void AddDungeon()
    {
        DungeonInfo dungeonInfo = new DungeonInfo();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_objectTag))
        {
            dungeonInfo.AddObject(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_monsterTag))
        {
            dungeonInfo.AddMonster(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalTranportTag))
        {
            dungeonInfo.AddPotalTransport(obj);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalSceneTag))
        {
            dungeonInfo.AddPotalScene(obj);
        }

        GameObject spotObject = GameObject.FindGameObjectWithTag("PlayerStartSpot");
        if (spotObject != null)
        {
            dungeonInfo.PlayerStartPosition = spotObject.transform.position;
        }
        else
        {
            dungeonInfo.PlayerStartPosition = new Vector3(0, 0, 0);
        }

        jsonData.dungeonObjectList.Add(dungeonInfo);
    }

    public T JsonLoad<T>(string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath + "\\Map\\", fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    private void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}
