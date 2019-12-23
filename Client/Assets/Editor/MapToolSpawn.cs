using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapToolSpawn
{
    private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

    public delegate GameObject CallBack(Object prefab, Vector2 pos);

    private const string _objectTag = "FieldObject";
    private const string _monsterTag = "Monster";
    private const string _potalTranportTag = "PotalTransport";

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

    public void Spawn(DungeonInfo dungeon, CallBack callBack)
    {
        foreach (var item in dungeon.objectinfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = callBack(obj ,new Vector2(item.position.x,item.position.y));
        }


        foreach (var item in dungeon.monsterInfos)
        {
            GameObject obj = LoadResourceFromCache(item.filePath);
            obj = callBack(obj, new Vector2(item.position.x, item.position.y));
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
        }
        ClearCache();
    }
    public GameObject SpawnObject(GameObject prefab, Vector3 position)
    {
        GameObject obj = Object.Instantiate(prefab);
        obj.transform.position = position;
        return obj;
    }
    public void DestoryAllObjects()
    {
        List<GameObject> ListObject = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_objectTag))
        {
            ListObject.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_monsterTag))
        {
            ListObject.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(_potalTranportTag))
        {
            ListObject.Add(obj);
        }
        GameObject spotObject = GameObject.FindGameObjectWithTag("PlayerStartSpot");
        if (spotObject != null)
        {
            ListObject.Add(spotObject);
        }

        foreach (var item in ListObject)
        {
            Object.DestroyImmediate(item);
        }
    }
}
