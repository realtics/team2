﻿using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Newtonsoft.Json;

public class ObjectInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
}
public class MonsterInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
}

public class PotalTransportinfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
    public Vector3[] spotPosition { get; set; }
    public ARROW arrow { get; set; }
    public int nextIndex { get; set; }
}

public class PotalSceneInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
    public Vector3 transportPosition { get; set; }
    public ARROW arrow { get; set; }
    public string nextDataName { get; set; }
}


public class DungeonInfo
{
    public List<ObjectInfo> objectinfos = new List<ObjectInfo>();
    public List<MonsterInfo> monsterInfos = new List<MonsterInfo>();
    public List<PotalTransportinfo> potalTransportinfos = new List<PotalTransportinfo>();
    public List<PotalSceneInfo> potalSceneInfos = new List<PotalSceneInfo>();

    public Vector3 PlayerStartPosition { get; set; }
}
public class JsonData
{
    public List<DungeonInfo> dungeonObjectList = new List<DungeonInfo>();
}

public class DungeonJsonData
{
    public DungeonInfo[] DungeonInfos;
}

public class MapLoader : MonoBehaviour
{
    private static MapLoader _instance;
    public static MapLoader instacne
    {
        get
        {
            return _instance;
        }
    }

    public DungeonJsonData dungeonData;

    private Dictionary<int,List<GameObject>> _dungeonGameObject;

    private string _dungeonName;
    private int _currentDungeonIndex;
    private const int _startDungeonIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _instance = this;
        _dungeonGameObject = new Dictionary<int, List<GameObject>>();
    }

    public void LoaderDungeon()
    {
        dungeonData = JsonLoad<DungeonJsonData>(_dungeonName);
    }
    public DungeonInfo GetDungeonInfo(int index)
    {
        return dungeonData.DungeonInfos[index];
    }
    public void SetMap(string dungeonName)
    {
        _dungeonName = dungeonName;
    }

    private T JsonLoad<T>(string fileName)
    {
        string path = string.Format("{0}/{1}", "Map", fileName);
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        string dungeonText = textAsset.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }
}
