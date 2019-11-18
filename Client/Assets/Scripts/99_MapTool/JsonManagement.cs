using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class JsonData
{
    public string filePath;
    public Vector3 position;

    public JsonData(string path, Vector3 pos)
    {
        filePath = path;
        position = pos;
    }

}

public class JsonManagement
{
    List<JsonData> _objectList;


    public JsonManagement()
    {
        _objectList = new List<JsonData>();
    }

    public void JsonSave()
    {
        _objectList.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            //Debug.Log(obj.name);
            AddObject(obj);
        }

        string jsonData = ObjectToJson(_objectList[0]);

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
        return _objectList[index];
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
        foreach (JsonData data in _objectList)
        {
            //GameObject obj = (GameObject)Resources.Load(data._filePath);
            GameObject obj = (GameObject)Resources.Load("Object\\StoneBar");
            obj.transform.position = data.position;

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
        _objectList.Add(data);
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

