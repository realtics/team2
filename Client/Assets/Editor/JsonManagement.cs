using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


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
    }
    public void JsonLoad(string fileName)
    {
        


    }
    public void InstObject()
    {
        foreach (JsonData data in _ObjectList)
        {
            ////GameObject obj = (GameObject)Resources.Load(data._filePath);
            //GameObject obj = (GameObject)Resources.Load("Object\\StoneBar");
            //obj.transform.position = data.position;

            //GameObject.Instantiate(obj);
            
        }
    }

    public void AddObject(GameObject obj)
    {
        //Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(obj);
        //string path = AssetDatabase.GetAssetPath(parentObject);
        //Debug.Log(path);

        //JsonData data = new JsonData(path, obj.transform.position);
        //_ObjectList.Add(data);
    }
}
