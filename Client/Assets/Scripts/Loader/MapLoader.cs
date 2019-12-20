using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Newtonsoft.Json;
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}
public class ObjectInfo
{
    public string filePath { get; set; }
    public SerializableVector3 position { get; set; }
}
public class MonsterInfo
{
    public string filePath { get; set; }
    public SerializableVector3 position { get; set; }
}

public class PotalTransportinfo
{
    public string filePath { get; set; }
    public SerializableVector3 position { get; set; }
    public SerializableVector3[] spotPosition { get; set; }
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
    public SerializableVector3 PlayerStartPosition { get; set; }
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
        SpawnManager.instance.LoadMapAssetBundle(_mapFolderName);
 
        TextAsset textAsset = SpawnManager.instance.LoadObjectAsset(fileName) as TextAsset;

        string dungeonText = textAsset.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }
}
