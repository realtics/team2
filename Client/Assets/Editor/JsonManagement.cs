using UnityEngine;
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
    private const string _potalTranportTag = "PotalTransport";

    private Vector2 _currentPosition = new Vector2(0,0);
    private bool _isBoss = false;

    public JsonManagement()
    {
        jsonData = new JsonData();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public void NewJson()
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile(Application.dataPath, "New", strJsonData);
    }

    public void SaveJson()
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile(Application.dataPath, "Test2", strJsonData);
    }

    public void ClearJson()
    {
        jsonData.dungeonObjectList.Clear();
        FirstPosition();
    }
    public void FirstPosition()
    {
        _currentPosition.Set(0, 0);
    }

    public void SetDungeonBoss()
    {
        if (_isBoss)
            _isBoss = false;
        else
            _isBoss = true;
        Debug.Log("Boss 던전 :" + _isBoss);
    }

    public void AddDungeon()
    {
        DungeonInfo dungeonInfo = new DungeonInfo();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_objectTag))
        {
            AddObject(dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_monsterTag))
        {
            AddMonster(dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalTranportTag))
        {
            AddPotalTransport(dungeonInfo, obj);
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

        dungeonInfo.position = _currentPosition;
        dungeonInfo.isBoss = _isBoss;
        // Todo 아래 부분을 맵툴 쪽으로 빼서 수동으로 조작하게 끔 해야함.
        Debug.Log(_currentPosition);
        _currentPosition.Set(_currentPosition.x + 1, _currentPosition.y);

        jsonData.dungeonObjectList.Add(dungeonInfo);
    }

    public T LoadJson<T>(string fileName)
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


    public void AddObject(DungeonInfo dungeonInfo,GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.filePath = path;
        objectInfo.position = obj.transform.position;

        dungeonInfo.objectinfos.Add(objectInfo);
    }

    public void AddMonster(DungeonInfo dungeonInfo, GameObject obj) 
    {
        //Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);

        MonsterInfo monsterInfo = new MonsterInfo();
        //monsterInfo.filePath = parentObject.name;
        monsterInfo.filePath = obj.name;
        monsterInfo.position = obj.transform.position;

        dungeonInfo.monsterInfos.Add(monsterInfo);
    }

    public void AddPotalTransport(DungeonInfo dungeonInfo, GameObject obj)
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

    string GetSubstringResourcesLoadFilePath(string filePath)
    {
        int FilePos = filePath.LastIndexOf("Resources/") + 10;
        string DirectoryFile = filePath.Substring(FilePos);
        int TagPos = DirectoryFile.IndexOf('/');
        if (TagPos > 0)
        {
            string Tagname = DirectoryFile.Remove(TagPos);
        }
        DirectoryFile = DirectoryFile.Remove(DirectoryFile.LastIndexOf('.'));
        return DirectoryFile;
    }
}
