using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

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
    List<JsonData> ObjectList;


    public JsonManagement()
    {
        ObjectList = new List<JsonData>();
    }

    public void JsonSave()
    {
        ObjectList.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FieldObject"))
        {
            //Debug.Log(obj.name);
            AddObject(obj);
        }
    }
    public void JsonLoad()
    {
        
    }

    public void InstObject()
    {
        foreach (JsonData data in ObjectList)
        {
            //GameObject obj = (GameObject)Resources.Load(data._filePath);
            GameObject obj = (GameObject)Resources.Load("Object\\StoneBar");
            obj.transform.position = data._position;

            GameObject.Instantiate(obj);
            
        }
    }

    public void AddObject(GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        Debug.Log(path);

        JsonData data = new JsonData(path, obj.transform.position);
        ObjectList.Add(data);
    }
}
