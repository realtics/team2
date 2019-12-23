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
    private const string _customMapFolderPath = "\\CustomMap\\";

    public JsonManagement()
    {
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public void NewJson(string fileName)
    {
        JsonData jsonData = new JsonData();
        DungeonInfo dungeonInfo = new DungeonInfo();
        dungeonInfo.position = new Vector2(0, 0);
        jsonData.dungeonObjectList.Add(dungeonInfo);

        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();
        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile(fileName, strJsonData, _customMapFolderPath);
    }

    public void SaveJson(JsonData jsonData, string fileName)
    {
        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonData.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile(fileName, strJsonData, _customMapFolderPath);
    }
    public void ExportJson(JsonData jsonData)
    {
        JsonData jsonDungeon = new JsonData();

        foreach(var item in jsonData.dungeonObjectList)
        {
            DungeonInfo dungeonInfo = new DungeonInfo();
            foreach(var obejctitem in item.objectinfos)
            {
                dungeonInfo.objectinfos.Add(obejctitem);
            }

            foreach (var monsteritem in item.monsterInfos)
            {
                MonsterInfo monsterInfo = new MonsterInfo();
                int TagPos = monsteritem.filePath.IndexOf('/') + 1;
                if (TagPos > 0)
                {
                    monsterInfo.filePath = monsteritem.filePath.Substring(TagPos);
                }
                monsterInfo.position = monsteritem.position;
                dungeonInfo.monsterInfos.Add(monsterInfo);
            }

            foreach (var potalitem in item.potalTransportinfos)
            {
                PotalTransportinfo potal = new PotalTransportinfo();
                int TagPos = potalitem.filePath.IndexOf('/') + 1;
                if (TagPos > 0)
                {
                    potal.filePath = potalitem.filePath.Substring(TagPos);
                }
                potal.position = potalitem.position;
                potal.arrow = potalitem.arrow;
                potal.nextIndex = potalitem.nextIndex;
                
                potal.spotPosition = new SerializableVector3[potalitem.spotPosition.Length];
                for (int i = 0; i < potalitem.spotPosition.Length; ++i)
                {
                    potal.spotPosition[i] = potalitem.spotPosition[i];
                }

                dungeonInfo.potalTransportinfos.Add(potal);
            }

            dungeonInfo.PlayerStartPosition = item.PlayerStartPosition;
            dungeonInfo.position = item.position;
            dungeonInfo.isBoss = item.isBoss;
            jsonDungeon.dungeonObjectList.Add(dungeonInfo);
        }

        DungeonJsonData dungeonJson = new DungeonJsonData();
        dungeonJson.DungeonInfos = jsonDungeon.dungeonObjectList.ToArray();

        string strJsonData = JsonConvert.SerializeObject(dungeonJson, setting);
        CreateJsonFile("New1", strJsonData, _mapFolderPath);
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
