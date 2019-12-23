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
        JsonData jsonData = new JsonData();
        DungeonInfo dungeonInfo = new DungeonInfo();
        dungeonInfo.position = new Vector2(0, 0);
        jsonData.dungeonObjectList.Add(dungeonInfo);

        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();
        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile("New", strJsonData);
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
        

        for (int i = 0; i < jsonData.dungeonObjectList.Count; i++)
        {
            foreach (var item in jsonData.dungeonObjectList[i].monsterInfos)
            {
                int TagPos = item.filePath.IndexOf('/') + 1;
                if (TagPos > 0)
                {
                    item.filePath = item.filePath.Substring(TagPos);
                }
            }
            foreach (var item in jsonData.dungeonObjectList[i].potalTransportinfos)
            {
                int TagPos = item.filePath.IndexOf('/') + 1;
                if (TagPos > 0)
                {
                    item.filePath = item.filePath.Substring(TagPos);
                }
            }
        }

        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile("New", strJsonData, _mapFolderPath);
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
