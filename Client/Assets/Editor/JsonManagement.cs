using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;


public class ObjectInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
}

public class Potalinfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
    public Vector3 transportPosition { get; set; }
    public string FilePathNextDungeon { get; set; }
}

public class DungeonInfo
{
    public List<ObjectInfo> objectinfos = new List<ObjectInfo>();
    public List<Potalinfo> potalinfos = new List<Potalinfo>();
    public Vector3 PlayerStartPosition { get; set; }

    public void AddObject(GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);

        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.filePath = path;
        objectInfo.position = obj.transform.position;

        objectinfos.Add(objectInfo);
    }

    public void AddPotal(GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);

        Potalinfo potalinfo = new Potalinfo();
        potalinfo.filePath = path;
        potalinfo.position = obj.transform.position;

        // 임시 데이터.
        potalinfo.transportPosition = obj.transform.position;
        potalinfo.FilePathNextDungeon = "Test1.json";

        potalinfos.Add(potalinfo);

    }
}

public class JsonData
{
    public List<DungeonInfo> dungeonObjectList = new List<DungeonInfo>();
}

public class DungeonJsonData
{ 
    public DungeonInfo[] DungeonInfos;
}


public class JVector3
{
    [JsonProperty("x")]
    public float x;
    [JsonProperty("y")]
    public float y;
    [JsonProperty("z")]
    public float z;

    public JVector3()
    {
        x = y = z = 0f;
    }

    public JVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public JVector3(float f)
    {
        x = y = z = f;
    }
}

public class JsonManagement
{
    JsonData jsonData = new JsonData();
    JsonSerializerSettings setting = new JsonSerializerSettings();
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
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            dungeonInfo.AddObject(obj);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            dungeonInfo.AddPotal(obj);
        }

        // 임시 데이터.
        //GameObject.FindGameObjectsWithTag("PlayerStart");
        dungeonInfo.PlayerStartPosition = new Vector3(0, 0, 0);

        jsonData.dungeonObjectList.Add(dungeonInfo);
    }


    public T JsonLoad<T>(string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath + "\\Map\\", fileName), FileMode.Open);
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

