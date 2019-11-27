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

public class PotalSceneInfo
{
    public string filePath { get; set; }
    public Vector3 position { get; set; }
    public Vector3 transportPosition { get; set; }
    public ARROW arrow { get; set; }
    public string nextDataName { get; set; }
}


public class DungeonInfo
{
    public List<ObjectInfo> objectinfos = new List<ObjectInfo>();
    public List<MonsterInfo> monsterInfos = new List<MonsterInfo>();
    public List<PotalTransportinfo> potalTransportinfos = new List<PotalTransportinfo>();
    public List<PotalSceneInfo> potalSceneInfos = new List<PotalSceneInfo>();

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

    public void AddMonster(GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        MonsterInfo monsterInfo = new MonsterInfo();
        monsterInfo.filePath = path;
        monsterInfo.position = obj.transform.position;

        monsterInfos.Add(monsterInfo);
    }

    public void AddPotalTransport(GameObject obj)
    {
        PotalTransport potal = obj.GetComponent<PotalTransport>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        PotalTransportinfo potalTransportinfo = new PotalTransportinfo();
        potalTransportinfo.filePath = path;
        potalTransportinfo.position = obj.transform.position;
        potalTransportinfo.arrow = potal.arrow;

        potalTransportinfo.spotPosition = new Vector3[potal.spotGatePosition.Length];
        for(int i = 0; i < potal.spotGatePosition.Length; ++i)
        {
            potalTransportinfo.spotPosition[i] = potal.spotGatePosition[i].position;
        }

        potalTransportinfo.nextIndex = potal.nextIndex;

        potalTransportinfos.Add(potalTransportinfo);
    }

    public void AddPotalScene(GameObject obj)
    {
        PotalScene potal = obj.GetComponent<PotalScene>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = ResourcesLoadSubstringFilePath(path);

        PotalSceneInfo potalSceneInfo = new PotalSceneInfo();
        potalSceneInfo.filePath = path;
        potalSceneInfo.position = obj.transform.position;
        potalSceneInfo.arrow = potal.arrow;
       
        potalSceneInfo.nextDataName = potal.nextSceneName;

        potalSceneInfos.Add(potalSceneInfo);
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

public class MapLoader : MonoBehaviour
{
    private static MapLoader _instance;
    public static MapLoader instacne
    {
        get
        {
            return _instance;
        }
    }

    public DungeonJsonData dungeonData;

    private Dictionary<int,List<GameObject>> _dungeonGameObject;

    private string _dungeonName;
    private int _currentDungeonIndex;
    private const int _startDungeonIndex = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _instance = this;
        _dungeonGameObject = new Dictionary<int, List<GameObject>>();
    }

    private void LoaderDungeon(string dungeonName)
    {
        dungeonData = JsonLoad<DungeonJsonData>(dungeonName);
    }
    private void LoaderMonster()
    {

    }

    private void Instantiate(int index)
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

            // ToDo. _MonsterManagerMent 
            foreach (var item in dungeon.monsterInfos)
            {
                GameObject obj = ObjectCache.instance.LoadResourceFromCache(item.filePath);
                MonsterManager.Instance.AddMonster(obj, item.position);
            }

            foreach (var item in dungeon.potalTransportinfos)
            {
                var obj = GameObject.Instantiate<GameObject>(ObjectCache.instance.LoadResourceFromCache(item.filePath));
                obj.transform.position = item.position;
                PotalTransport potal = obj.GetComponent<PotalTransport>();
                potal.arrow = item.arrow;
                potal.nextIndex = item.nextIndex;

                for(int i = 0; i < item.spotPosition.Length; i++)
                {
                    potal.spotGatePosition[i].position = item.spotPosition[i];
                }

                _dungeonGameObject[index].Add(obj);
            }
            foreach (var item in dungeon.potalSceneInfos)
            {
                var obj = GameObject.Instantiate<GameObject>(ObjectCache.instance.LoadResourceFromCache(item.filePath));
                obj.transform.position = item.position;
                PotalScene potal = obj.GetComponent<PotalScene>();
                potal.arrow = item.arrow;
                potal.nextSceneName = item.nextDataName;

                _dungeonGameObject[index].Add(obj);
            }


            ObjectCache.instance.ClearCache();
        }
        else
        {
            RoomSetActive(true, index);
        }
    }
    private void RoomSetActive(bool active,int index)
    {
        foreach (var item in _dungeonGameObject[index])
        {
            item.gameObject.SetActive(active);
        }
    }

    public void ChangeRoom(int index, ARROW arrow)
    {
        GameManager gameManager = GetGameManager();
        gameManager.FadeOut();

        RoomSetActive(false, _currentDungeonIndex);
        Instantiate(index);
        _currentDungeonIndex = index;

        PotalManager potalManager = PotalManager.instance;
        potalManager.FIndPotals();
        potalManager.BlockPotals();

        gameManager.FindCameraCollider();
        gameManager.MoveToPlayer(potalManager.FindGetArrowPotalPosition(FlipArrow(arrow)));
    }
    private ARROW FlipArrow(ARROW arrow)
    {
        switch(arrow)
        {
            case ARROW.UP:
                {
                    return ARROW.DOWN;
                }
            case ARROW.DOWN:
                {
                    return ARROW.UP;
                }
            case ARROW.LEFT:
                {
                    return ARROW.RIGHT;
                }
            case ARROW.RIGHT:
                {
                    return ARROW.LEFT;
                }
            default:
                {
                    return ARROW.NULL;
                }
        }
    }
     
    public void Loader()
    {
        const int startDungeonIndex = 0;

        GameManager gameManager = GetGameManager();
        gameManager.FadeOut();

        LoaderDungeon(_dungeonName);
        _currentDungeonIndex = _startDungeonIndex;
        Instantiate(_currentDungeonIndex);

        gameManager.FindCameraCollider();
        gameManager.MoveToPlayer(dungeonData.DungeonInfos[startDungeonIndex].PlayerStartPosition);

    }
    public void SetMap(string dungeonName)
    {
        _dungeonName = dungeonName;
    }

    public T JsonLoad<T>(string fileName)
    {
        string path = string.Format("{0}/{1}", "Map", fileName);
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        string dungeonText = textAsset.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }

    private GameManager GetGameManager()
    {
        if(DungeonGameManager.Instance != null)
        {
            return DungeonGameManager.Instance;
        }
        else if(LobbyGameManager.Instance != null)
        {
            return LobbyGameManager.Instance;
        }
        return null;
    }
}
