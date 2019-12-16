using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnManager : MonoBehaviour
{
    private List<GameObject> _dungeonGameObject = new List<GameObject>();

    [SerializeField]
    private AssetBundleManager _assetBundleManager;

    private static SpawnManager _instacne;
    public static SpawnManager instacne
    {
        get
        {
            return _instacne;
        }
    }


    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        _instacne = this;
    }

    public GameObject LoadResourceFromCache(string path)
    {
        GameObject resourceObj = null;
        _cache.TryGetValue(path, out resourceObj);
        if (resourceObj == null)
        {
            resourceObj = Resources.Load<GameObject>(path);
            if (resourceObj != null)
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

    public void Spawn(DungeonInfo dungeon)
    {

        foreach (var item in dungeon.objectinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = AddObject(obj, item.position);

            _dungeonGameObject.Add(obj);
        }

        LoadAssetBundle("monster");
        foreach (var item in dungeon.monsterInfos)
        {
            GameObject obj = LoadAsset(item.filePath);

            MonsterManager.Instance.AddMonster(obj, item.position);
        }
        UnLoadAssetBundle(false);

        LoadAssetBundle("dungeon/potal");
        foreach (var item in dungeon.potalTransportinfos)
        {
            //GameObject obj = LoadResourceFromCache(item.filePath);

            GameObject obj = LoadAsset(item.filePath);

            obj = AddObject(obj, item.position);

            PotalTransport potal = obj.GetComponent<PotalTransport>();
            potal.arrow = item.arrow;
            potal.nextIndex = item.nextIndex;

            for (int i = 0; i < item.spotPosition.Length; i++)
            {
                potal.spotGatePosition[i].position = item.spotPosition[i];
            }

            _dungeonGameObject.Add(obj);
        }
        UnLoadAssetBundle(false);

        ClearCache();
    }

    public void LoadAssetBundle(string name)
    {
        _assetBundleManager.LoadAssetFromLocalDisk(name);
    }

    public GameObject LoadAsset(string name)
    {
        return _assetBundleManager.LoadAsset(name);
    }
    public Object LoadObjectAsset(string name)
    {
        return _assetBundleManager.LoadUnCacheObjectAsset(name);
    }
    public void UnLoadAssetBundle(bool isUnloadAll)
    {
        _assetBundleManager.UnLoadAssetBundle(isUnloadAll);
    }

    public GameObject AddObject(GameObject prefab, Vector3 position)
    {
        GameObject obj = ObjectPoolManager.Instance.GetRestObject(prefab);
        obj.transform.position = position;
        return obj;
    }
    public void RoomSetActive(bool active, int index)
    {
        foreach (var item in _dungeonGameObject)
        {
            item.gameObject.SetActive(active);
        }
    }
}
