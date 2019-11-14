using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class JsonData
{
    public string _filePath;
    public Vector3 _position;

    public JsonData(string path, Vector3 pos)
    {
        _filePath = path;
        _position = pos;
    }

}

public class JsonManagement
{
    List<JsonData> _ObjectList;


    public JsonManagement()
    {
        _ObjectList = new List<JsonData>();
    }

    public void JsonSave()
    {
        _ObjectList.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            //Debug.Log(obj.name);
            AddObject(obj);
        }

        string jsonData = ObjectToJson(_ObjectList[0]);

        CreateJsonFile(Application.dataPath, "Test1", jsonData);
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
    public JsonData GetObject(int index)
    {
        return _ObjectList[index];
    }
    //public JsonData FindObject(string objectName)
    //{
    //    // ing..

    //    foreach(JsonData item in _ObjectList)
    //    {
    //        //item._filePath
    //    }

    //    return null;
    //}
    public void InstObject()
    {
        foreach (JsonData data in _ObjectList)
        {
            //GameObject obj = (GameObject)Resources.Load(data._filePath);
            GameObject obj = (GameObject)Resources.Load("Object\\StoneBar");
            obj.transform.position = data._position;

            GameObject.Instantiate(obj);

        }
    }

    public void AddObject(GameObject obj)
    {
        //Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        //string path = AssetDatabase.GetAssetPath(parentObject);
        string path = "Object\\far2Background";
        Debug.Log(path);

        JsonData data = new JsonData(path, obj.transform.position);
        _ObjectList.Add(data);
    }

    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    T JsonToOject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}

