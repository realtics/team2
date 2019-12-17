using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Newtonsoft.Json;

public class ObjectInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
}
public class MonsterInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
}

public class PotalTransportinfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
    public Vector3[] spotPosition { get; set; }
    public ARROW arrow { get; set; }
    public int nextIndex { get; set; }
}

public class DungeonInfo
{
    public Vector2 position;
    public List<ObjectInfo> objectinfos = new List<ObjectInfo>();
    public List<MonsterInfo> monsterInfos = new List<MonsterInfo>();
    public List<PotalTransportinfo> potalTransportinfos = new List<PotalTransportinfo>();
    public bool isBoss;
    public Vector3 PlayerStartPosition { get; set; }
}
public class JsonData
{
    public List<DungeonInfo> dungeonObjectList = new List<DungeonInfo>();
}

public class DungeonJsonData
{
    public DungeonInfo[] DungeonInfos;
}

public class MapLoader : Single.Singleton<MapLoader>
{
    public DungeonJsonData dungeonData;
    private string _dungeonName;
    private string _mapFolderName = "map";

    public void LoaderDungeon()
    {
        dungeonData = JsonLoad<DungeonJsonData>(_dungeonName);
        SpawnManager.instance.UnLoadAssetBundle(false);
    }
    public DungeonInfo GetDungeonInfo(int index)
    {
        return dungeonData.DungeonInfos[index];
    }
    public void SetMap(string dungeonName)
    {
        _dungeonName = dungeonName;
    }
    public void AfterInstantiateMonsterDelete(int index)
    {
        dungeonData.DungeonInfos[index].monsterInfos.Clear();
    }

    private T JsonLoad<T>(string fileName)
    {
        SpawnManager.instance.LoadAssetBundle(_mapFolderName);
 
        TextAsset textAsset = SpawnManager.instance.LoadObjectAsset(fileName) as TextAsset;

        string dungeonText = textAsset.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }
}
