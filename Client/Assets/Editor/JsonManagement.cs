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

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalSceneTag))
        {
            AddPotalScene(dungeonInfo, obj);
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


    public void AddObject(DungeonInfo dungeonInfo,GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.filePath = path;
        objectInfo.position = obj.transform.position;

        dungeonInfo.objectinfos.Add(objectInfo);
    }

    public void AddMonster(DungeonInfo dungeonInfo, GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        MonsterInfo monsterInfo = new MonsterInfo();
        monsterInfo.filePath = path;
        monsterInfo.position = obj.transform.position;

        dungeonInfo.monsterInfos.Add(monsterInfo);
    }

    public void AddPotalTransport(DungeonInfo dungeonInfo, GameObject obj)
    {
        PotalTransport potal = obj.GetComponent<PotalTransport>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        PotalTransportinfo potalTransportinfo = new PotalTransportinfo();
        potalTransportinfo.filePath = path;
        potalTransportinfo.position = obj.transform.position;
        potalTransportinfo.arrow = potal.arrow;

        potalTransportinfo.spotPosition = new Vector3[potal.spotGatePosition.Length];
        for (int i = 0; i < potal.spotGatePosition.Length; ++i)
        {
            potalTransportinfo.spotPosition[i] = potal.spotGatePosition[i].position;
        }

        potalTransportinfo.nextIndex = potal.nextIndex;

        dungeonInfo.potalTransportinfos.Add(potalTransportinfo);
    }

    public void AddPotalScene(DungeonInfo dungeonInfo, GameObject obj)
    {
        PotalScene potal = obj.GetComponent<PotalScene>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        PotalSceneInfo potalSceneInfo = new PotalSceneInfo();
        potalSceneInfo.filePath = path;
        potalSceneInfo.position = obj.transform.position;
        potalSceneInfo.arrow = potal.arrow;

        potalSceneInfo.nextDataName = potal.nextSceneName;

        dungeonInfo.potalSceneInfos.Add(potalSceneInfo);
    }

    string ResourcesLoadSubstringFilePath(string FilePath)
    {
        int FilePos = FilePath.LastIndexOf("Resources/") + 10;
        string DirectoryFile = FilePath.Substring(FilePos);
        int TagPos = DirectoryFile.IndexOf('/');
        if (TagPos > 0)
        {
            string Tagname = DirectoryFile.Remove(TagPos);
        }
        DirectoryFile = DirectoryFile.Remove(DirectoryFile.LastIndexOf('.'));
        return DirectoryFile;
    }
}
