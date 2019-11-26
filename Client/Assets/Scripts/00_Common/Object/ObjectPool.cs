using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool
{
    private string _poolName;
    public GameObject pooledObject;
    public int initObjectCount;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    public string PoolName { get { return _poolName; } }

    public void Start()
    {
        _poolName = pooledObject.name;
    }

    public void CreatePool(GameObject prefab)
    {
        pooledObject = prefab;
        _poolName = pooledObject.name;
    }

    public GameObject GetObject()
    {
        GameObject obj = FindRestObject();

        if (obj == null)
        {
            Debug.LogError("오브젝트 불러오기 실패!");
            return null;
        }

        obj.gameObject.SetActive(true);

        return obj;
    }

    public GameObject GetObject(Vector3 position)
    {
        GameObject obj = GetObject();

        if (obj == null)
            return null;

        obj.transform.position = position;

        return obj;
    }

    public GameObject GetObject(Transform parent, Vector3 position)
    {
        GameObject obj = GetObject();

        if (obj == null)
            return null;

        obj.transform.SetParent(parent);
        obj.transform.position = position;

        return obj;
    }

    public GameObject CreateObject()
    {
        if (pooledObject == null)
        {
            Debug.LogError("생성 할 프리팹이 없습니다.");
            return null;
        }

        GameObject newObject = GameObject.Instantiate(pooledObject);

        if (newObject == null)
            return null;

        newObject.SetActive(false);

        _spawnedObjects.Add(newObject);

        return newObject;
    }

    private GameObject FindRestObject()
    {
        foreach (GameObject obj in _spawnedObjects)
        {
            if (obj.gameObject.activeInHierarchy)
                continue;

            return obj;
        }

        return CreateObject();
    }
}
