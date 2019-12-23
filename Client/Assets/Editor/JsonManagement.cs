using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;


public class JsonManagement
{
    JsonSerializerSettings setting = new JsonSerializerSettings();

    private const string _mapFolderPath = "\\Map\\";

    public JsonManagement()
    {
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public void NewJson()
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile(Application.dataPath, "New", strJsonData);
    }

    public void SaveJson(JsonData jsonData)
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile("New", strJsonData);
    }
    public void ExportJson(JsonData jsonData)
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile("New", strJsonData, _mapFolderPath);
    }
    public T LoadJson<T>(string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath + _mapFolderPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    private void CreateJsonFile(string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    private void CreateJsonFile(string fileName, string jsonData, string addFrontPath)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath + addFrontPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}
