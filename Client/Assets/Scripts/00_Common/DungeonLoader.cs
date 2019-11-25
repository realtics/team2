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

public class Potalinfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
    public Vector3 transportPosition { get; set; }
    public ARROW Arrow { get; set; }
    public int NextDungeonIndex { get; set; }
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
        path = ResourcesLoadSubstringFilePath(path);

        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.filePath = path;
        objectInfo.position = obj.transform.position;

        objectinfos.Add(objectInfo);
    }

    public void AddPotal(GameObject obj)
    {
        Potal potal = obj.GetComponent<Potal>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        Potalinfo potalinfo = new Potalinfo();
        potalinfo.filePath = path;
        potalinfo.position = obj.transform.position;
        potalinfo.Arrow = potal.arrow;

        potalinfo.transportPosition = potal.transportPosition;
        potalinfo.NextDungeonIndex = potal.nextDungenIndex;

        potalinfos.Add(potalinfo);
    }

    string ResourcesLoadSubstringFilePath(string FilePath)
    {
        int FilePos = FilePath.LastIndexOf("Resources/") + 10;
        string DirectoryFile = FilePath.Substring(FilePos);
        int TagPos = DirectoryFile.IndexOf('/');
        if (TagPos > 0)
        {
            string Tagname = DirectoryFile.Remove(TagPos);
        }
        DirectoryFile = DirectoryFile.Remove(DirectoryFile.LastIndexOf('.'));
        return DirectoryFile;
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

public class ObjectCache : ScriptableSingleton<ObjectCache>
{
    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

    public GameObject LoadResourceFromCache(string path)
    {
        GameObject resourceObj = null;
        _cache.TryGetValue(path, out resourceObj);
        if(resourceObj == null)
        {
            resourceObj = Resources.Load<GameObject>(path);
            if(resourceObj != null)
            {
                _cache.Add(path, resourceObj);
            }
        }
        return resourceObj;
    }
    public void ClearCache()
    {
        _cache.Clear();
        Resources.UnloadUnusedAssets();
    }
}

public class DungeonLoader : MonoBehaviour
{
    private static DungeonLoader _instance;
    public static DungeonLoader instacne
    {
        get
        {
            return _instance;
        }
    }

    public DungeonJsonData dungeonData;

    //private List<List<GameObject>> _dungeonGameObject;
    private Dictionary<int,List<GameObject>> _dungeonGameObject;

    private void Start()
    {
        _instance = this;
        _dungeonGameObject = new Dictionary<int, List<GameObject>>();
    }

    public void Loader(string dungeonName)
    {
        dungeonData = JsonLoad<DungeonJsonData>(dungeonName);
    }

    public void Instantiate(int index)
    {
        if (_dungeonGameObject.ContainsKey(index) == false)
        {
            List<GameObject> objects = new List<GameObject>();
            _dungeonGameObject.Add(index, objects);

            DungeonInfo dungeon = dungeonData.DungeonInfos[index];

            foreach (var item in dungeon.objectinfos)
            {
                var obj = GameObject.Instantiate<GameObject>(ObjectCache.instance.LoadResourceFromCache(item.filePath));
                obj.transform.position = item.position;
                _dungeonGameObject[index].Add(obj);
            }
            foreach (var item in dungeon.potalinfos)
            {
                var obj = GameObject.Instantiate<GameObject>(ObjectCache.instance.LoadResourceFromCache(item.filePath));
                obj.transform.position = item.position;
                Potal potal = obj.GetComponent<Potal>();
                potal.arrow = item.Arrow;
                potal.nextDungenIndex = item.NextDungeonIndex;
                potal.transportPosition = item.transportPosition;
                _dungeonGameObject[index].Add(obj);
            }
            ObjectCache.instance.ClearCache();
        }
        else
        {
            foreach(var item in _dungeonGameObject[index])
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    

    public void Test(int index)
    {
        Loader("Test2");
        Instantiate(index);
    }

    public T JsonLoad<T>(string fileName)
    {
        string path = string.Format("{0}/{1}", "Map", fileName);
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        string dungeonText = textAsset.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }
}
