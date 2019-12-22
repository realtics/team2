using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapToolSpawn
{
    private List<GameObject> _dungeonGameObject = new List<GameObject>();

    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

    public delegate GameObject CallBack(Object prefab, Vector2 pos);

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
            obj = SpawnObject(obj, item.position);

            _dungeonGameObject.Add(obj);
        }


        foreach (var item in dungeon.monsterInfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = SpawnObject(obj, item.position);

            _dungeonGameObject.Add(obj);
        }

        foreach (var item in dungeon.potalTransportinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);

            obj = SpawnObject(obj, item.position);

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
    public void Spawn(DungeonInfo dungeon, CallBack callBack)
    {
        foreach (var item in dungeon.objectinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = callBack(obj ,new Vector2(item.position.x,item.position.y));

            _dungeonGameObject.Add(obj);
        }


        foreach (var item in dungeon.monsterInfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = callBack(obj, new Vector2(item.position.x, item.position.y));

            _dungeonGameObject.Add(obj);
        }

        foreach (var item in dungeon.potalTransportinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);

            obj = callBack(obj, new Vector2(item.position.x, item.position.y));

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
    public GameObject SpawnObject(GameObject prefab, Vector3 position)
    {
        GameObject obj = Object.Instantiate(prefab);
        obj.transform.position = position;
        return obj;
    }
    public void SetActiveRoom(bool active, int index)
    {
        foreach (var item in _dungeonGameObject)
        {
            item.gameObject.SetActive(active);
        }
    }
    public void DestoryAllObject()
    {
        foreach (var item in _dungeonGameObject)
        {
            Object.DestroyImmediate(item);
        }
    }
    public void ClearListdungeonObject()
    {
        _dungeonGameObject.Clear();
    }
}
