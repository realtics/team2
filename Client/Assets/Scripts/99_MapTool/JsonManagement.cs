using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class DungeonInfo
{
    public string filePath;

    public class ObjectInfo
    {
        public string filePath;
        public Vector3 position;
    }

    public class Potalinfo
    {
        public string filePath;
        public Vector3 position;
        public Vector3 transportPosition;
        public string FilePathNextDungeon;
    }


    public List<ObjectInfo> objectinfos;
    public List<Potalinfo> potalinfos;
    public Vector3 PlayerStartPosition;

    public DungeonInfo()
    {
        objectinfos = new List<ObjectInfo>();
        potalinfos = new List<Potalinfo>();
    }
}

public class JsonData
{
    List<DungeonInfo> _dungeonObjectList;

    public JsonData()
    {
        _dungeonObjectList = new List<DungeonInfo>();
    }

    public void Add(DungeonInfo dungeonInfo)
    {
        _dungeonObjectList.Add(dungeonInfo);
    }

    public void Clear()
    {
        _dungeonObjectList.Clear();
    }
}

public class JsonManagement
{
    JsonData jsonData = new JsonData();


    public JsonManagement()
    {
        jsonData = new JsonData();
    }

    public void JsonSave()
    {
        jsonData.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            //Debug.Log(obj.name);
            AddObject(obj);
        }

        string strJsonData = ObjectToJson(jsonData);

        CreateJsonFile(Application.dataPath, "Test1", strJsonData);
    }
    public T JsonLoad<T>(string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath+"\\Map\\", fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void InstObject()
    {
        //foreach (JsonData data in jsonData)
        //{
        //    GameObject obj = (GameObject)Resources.Load(data._filePath);
        //    obj.transform.position = data.position;

        //    GameObject.Instantiate(obj);

        //}
    }

    public void AddObject(GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        //string path = "Object\\far2Background";
        //Debug.Log(path);

        DungeonInfo dungeonInfo = new DungeonInfo();

        //dungeonInfo.objectinfos.Add( );
        //dungeonInfo.potalinfos.Add();


        jsonData.Add(dungeonInfo);
    }

    private string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    private T JsonToOject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    private void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}

