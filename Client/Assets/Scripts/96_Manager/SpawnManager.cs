using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _dungeonGameObject;

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
        _dungeonGameObject = new List<GameObject>();
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

    public void Instantiate(DungeonInfo dungeon)
    {
        foreach (var item in dungeon.objectinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = AddObject(obj, item.position);

            _dungeonGameObject.Add(obj);
        }

        foreach (var item in dungeon.monsterInfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            MonsterManager.Instance.AddMonster(obj, item.position);
            
        }

        foreach (var item in dungeon.potalTransportinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
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
