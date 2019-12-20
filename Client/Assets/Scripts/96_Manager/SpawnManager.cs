using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnManager : Single.Singleton<SpawnManager>
{
    private List<GameObject> _dungeonGameObject = new List<GameObject>();

    private AssetBundleManager _assetBundleManager;

    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

    private string _loadMonseterAssetName = "monster";
    private string _loadPotalAssetName = "dungeon/potal";
    private bool _firstLoadMonseterAssetbundle = false;
    private bool _firstLoadPotalAssetbundle = false;

    public SpawnManager()
    {
        _assetBundleManager = AssetBundleManager.instacne;
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

        if (!_firstLoadMonseterAssetbundle)
        {
            LoadMonsterAssetBundle(_loadMonseterAssetName);
            _firstLoadMonseterAssetbundle = true;
            Debug.Log(_firstLoadMonseterAssetbundle);
        }

        foreach (var item in dungeon.monsterInfos)
        {
            GameObject obj = LoadMonsterAsset(item.filePath);

            MonsterManager.Instance.AddMonster(obj, item.position);
        }

        if(!_firstLoadPotalAssetbundle)
        {
            LoadPotalAssetBundle(_loadPotalAssetName);
            _firstLoadPotalAssetbundle = true;
        }
        foreach (var item in dungeon.potalTransportinfos)
        {
            GameObject obj = LoadPotalAsset(item.filePath);

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
        ClearCache();
    }

    public void LoadMapAssetBundle(string name)
    {
        _assetBundleManager.LoadMapAssetFromLocalDisk(name);
    }
    public void LoadMonsterAssetBundle(string name)
    {
        _assetBundleManager.LoadMonsterAssetFromLocalDisk(name);
    }
    public void LoadPotalAssetBundle(string name)
    {
        _assetBundleManager.LoadPotalAssetFromLocalDisk(name);
    }
    public GameObject LoadMonsterAsset(string name)
    {
        return _assetBundleManager.LoadMonsterAsset(name);
    }
    public GameObject LoadPotalAsset(string name)
    {
        return _assetBundleManager.LoadPotalAsset(name);
    }
    public Object LoadMapObjectAsset(string name)
    {
        return _assetBundleManager.LoadMapObjectAsset(name);
    }

    public void UnLoadAssetBundle(bool isUnloadAll)
    {
        _assetBundleManager.UnLoadAssetBundle(isUnloadAll);
    }
    public void UnLoadMonsterAssetBundle(bool isUnloadAll)
    {
        _assetBundleManager.UnLoadMonserAssetBundle(isUnloadAll);
    }
    public void UnLoadPotalAssetBundle(bool isUnloadAll)
    {
        _assetBundleManager.UnLoadPotalAssetBundle(isUnloadAll);
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
    public void ClearListdungeonObject()
    {
        _dungeonGameObject.Clear();
    }
}
