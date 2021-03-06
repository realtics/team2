﻿using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;
using System.Collections.Generic;

public class MapToolLoader
{
    public JsonData dungeonData;
    private DungeonJsonData dungeonJsonData;
    private const string _objectTag = "FieldObject";
    private const string _monsterTag = "Monster";
    private const string _potalTranportTag = "PotalTransport";

    private const int _lastSubstringIndex = 10;

    public MapToolLoader()
    {
        dungeonData = new JsonData();
    }

    public void LoaderDungeon(TextAsset dungeon)
    {
        dungeonJsonData = JsonLoad<DungeonJsonData>(dungeon);
        dungeonData.dungeonObjectList.Clear();
        dungeonData.dungeonName = dungeonJsonData.dungeonName;
        for (int i = 0; i < dungeonJsonData.DungeonInfos.Length; i++)
        {
            dungeonData.dungeonObjectList.Add(dungeonJsonData.DungeonInfos[i]);
        }
    }
    public DungeonInfo GetDungeonInfo(int index)
    {
        return dungeonData.dungeonObjectList[index];
    }
    public void AddRoom()
    {
        Vector2 position;
 
        DungeonInfo dungeonInfo = new DungeonInfo();
         
        dungeonInfo.PlayerStartPosition = new Vector3(0, 0, 0);
        if (dungeonData.dungeonObjectList.Count > 0)
        {
            position = dungeonData.dungeonObjectList[dungeonData.dungeonObjectList.Count - 1].position;
            position.Set(position.x + 1, position.y);
            dungeonInfo.position = position;
        }
        else
        {
            dungeonInfo.position = new Vector2(0, 0);
        }
        dungeonInfo.isBoss = false;
        dungeonData.dungeonObjectList.Add(dungeonInfo);
    }

    public void DeleteRoom(int roomIndex)
    {
        dungeonData.dungeonObjectList.Remove(dungeonData.dungeonObjectList[roomIndex]);
    }
    public void SaveRoom(int roomIndex)
    {
        DungeonInfo dungeonInfo = dungeonData.dungeonObjectList[roomIndex];
        dungeonInfo.monsterInfos.Clear();
        dungeonInfo.objectinfos.Clear();
        dungeonInfo.potalTransportinfos.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_objectTag))
        {
            AddObject(ref dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_monsterTag))
        {
            AddMonster(ref dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalTranportTag))
        {
            AddPotalTransport(ref dungeonInfo, obj);
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
    }

    public void AddObject(ref DungeonInfo dungeonInfo, GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.filePath = path;
        objectInfo.position = obj.transform.position;

        dungeonInfo.objectinfos.Add(objectInfo);
    }

    public void AddMonster(ref DungeonInfo dungeonInfo, GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        MonsterInfo monsterInfo = new MonsterInfo();
        monsterInfo.filePath = path;
        monsterInfo.position = obj.transform.position;

        dungeonInfo.monsterInfos.Add(monsterInfo);
    }

    public void AddPotalTransport(ref DungeonInfo dungeonInfo, GameObject obj)
    {
        PotalTransport potal = obj.GetComponent<PotalTransport>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        PotalTransportinfo potalTransportinfo = new PotalTransportinfo();
        potalTransportinfo.filePath = path;
        potalTransportinfo.position = obj.transform.position;
        potalTransportinfo.arrow = potal.arrow;

        potalTransportinfo.spotPosition = new SerializableVector3[potal.spotGatePosition.Length];
        for (int i = 0; i < potal.spotGatePosition.Length; ++i)
        {
            potalTransportinfo.spotPosition[i] = potal.spotGatePosition[i].position;
        }

        potalTransportinfo.nextIndex = potal.nextIndex;

        dungeonInfo.potalTransportinfos.Add(potalTransportinfo);
    }

    public string GetSubstringResourcesLoadFilePath(string filePath)
    {     
        int FilePos = filePath.LastIndexOf("Resources/") + _lastSubstringIndex;
        string DirectoryFile = filePath.Substring(FilePos);
        int TagPos = DirectoryFile.IndexOf('/');
        if (TagPos > 0)
        {
            DirectoryFile.Remove(TagPos);
        }
        int extensionPos = DirectoryFile.LastIndexOf('.');
        if(extensionPos > 0)
        {
            DirectoryFile = DirectoryFile.Remove(DirectoryFile.LastIndexOf('.'));
        }
        return DirectoryFile;
    }

    private T JsonLoad<T>(TextAsset dungeon)
    {
        string dungeonText = dungeon.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }

    public void SetDungeonName(string name)
    {
        dungeonData.dungeonName = name;
    }
    public string GetDungeonName()
    {
        return dungeonData.dungeonName;
    }
}
