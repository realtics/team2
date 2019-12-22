using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

public class MapToolLoader
{
    public JsonData dungeonList;
    public DungeonJsonData dungeonData;
    private const string _objectTag = "FieldObject";
    private const string _monsterTag = "Monster";
    private const string _potalTranportTag = "PotalTransport";

    private Vector2 _currentPosition = new Vector2(0, 0);
    private bool _isBoss = false;

    public MapToolLoader()
    {
        dungeonList = new JsonData();
    }

    public void LoaderDungeon(TextAsset dungeon)
    {
        dungeonData = JsonLoad<DungeonJsonData>(dungeon);
        dungeonList.dungeonObjectList.Clear();
        for (int i = 0; i < dungeonData.DungeonInfos.Length; i++)
        {
            dungeonList.dungeonObjectList.Add(dungeonData.DungeonInfos[i]);
        }
    }
    public DungeonInfo GetDungeonInfo(int index)
    {
        return dungeonList.dungeonObjectList[index];
    }
    public void AddRoom()
    {
        DungeonInfo dungeonInfo = new DungeonInfo();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_objectTag))
        {
            AddObject(dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_monsterTag))
        {
            AddMonster(dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalTranportTag))
        {
            AddPotalTransport(dungeonInfo, obj);
        }
        GameObject spotObject = GameObject.FindGameObjectWithTag("PlayerStartSpot");
        if (spotObject != null)
        {
            dungeonInfo.PlayerStartPosition = spotObject.transform.position;
        }
        else
        {
            dungeonInfo.PlayerStartPosition = new Vector3(0, 0, 0);
        }
        dungeonInfo.position = _currentPosition;
        dungeonInfo.isBoss = _isBoss;
        // Todo 아래 부분을 맵툴 쪽으로 빼서 수동으로 조작하게 끔 해야함.
        Debug.Log(_currentPosition);
        _currentPosition.Set(_currentPosition.x + 1, _currentPosition.y);

        dungeonList.dungeonObjectList.Add(dungeonInfo);
    }
    public void SaveRoom(int roomIndex, bool isBoss)
    {
        DungeonInfo dungeonInfo = new DungeonInfo();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_objectTag))
        {
            AddObject(dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_monsterTag))
        {
            AddMonster(dungeonInfo, obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalTranportTag))
        {
            AddPotalTransport(dungeonInfo, obj);
        }
        GameObject spotObject = GameObject.FindGameObjectWithTag("PlayerStartSpot");
        if (spotObject != null)
        {
            dungeonInfo.PlayerStartPosition = spotObject.transform.position;
        }
        else
        {
            dungeonInfo.PlayerStartPosition = new Vector3(0, 0, 0);
        }
        dungeonInfo.position = dungeonList.dungeonObjectList[roomIndex].position;
        dungeonInfo.isBoss = isBoss;

        dungeonList.dungeonObjectList[roomIndex] = dungeonInfo;
    }

    public void AddObject(DungeonInfo dungeonInfo, GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.filePath = path;
        objectInfo.position = obj.transform.position;

        dungeonInfo.objectinfos.Add(objectInfo);
    }

    public void AddMonster(DungeonInfo dungeonInfo, GameObject obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        MonsterInfo monsterInfo = new MonsterInfo();
        monsterInfo.filePath = path;
        //monsterInfo.filePath = obj.name;
        monsterInfo.position = obj.transform.position;

        dungeonInfo.monsterInfos.Add(monsterInfo);
    }

    public void AddPotalTransport(DungeonInfo dungeonInfo, GameObject obj)
    {
        PotalTransport potal = obj.GetComponent<PotalTransport>();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
        string path = AssetDatabase.GetAssetPath(parentObject);
        path = GetSubstringResourcesLoadFilePath(path);

        PotalTransportinfo potalTransportinfo = new PotalTransportinfo();
        potalTransportinfo.filePath = path;
        potalTransportinfo.position = obj.transform.position;
        potalTransportinfo.arrow = potal.arrow;

        potalTransportinfo.spotPosition = new SerializableVector3[potal.spotGatePosition.Length];
        for (int i = 0; i < potal.spotGatePosition.Length; ++i)
        {
            potalTransportinfo.spotPosition[i] = potal.spotGatePosition[i].position;
        }

        potalTransportinfo.nextIndex = potal.nextIndex;

        dungeonInfo.potalTransportinfos.Add(potalTransportinfo);
    }

    string GetSubstringResourcesLoadFilePath(string filePath)
    {
        int FilePos = filePath.LastIndexOf("Resources/") + 10;
        string DirectoryFile = filePath.Substring(FilePos);
        int TagPos = DirectoryFile.IndexOf('/');
        if (TagPos > 0)
        {
            string Tagname = DirectoryFile.Remove(TagPos);
        }
        DirectoryFile = DirectoryFile.Remove(DirectoryFile.LastIndexOf('.'));
        return DirectoryFile;
    }

    private T JsonLoad<T>(TextAsset dungeon)
    {
        string dungeonText = dungeon.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }
}
