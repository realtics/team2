using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> _pools = new List<ObjectPool>();

    private static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        foreach (ObjectPool pool in _pools)
        {
            pool.Start();
            for (int i = 0; i < pool.initObjectCount; i++)
            {
                pool.CreateObject();
            }
        }
    }

    public bool CreatePool(GameObject prefab, int initCount = 0)
    {
        foreach (ObjectPool pool in _pools)
        {
            if (pool.pooledObjectPrefab == prefab)
                return false;
        }

        ObjectPool newPool = new ObjectPool();

        newPool.CreatePool(prefab);

        newPool.initObjectCount = initCount;

        for (int i = 0; i < newPool.initObjectCount; i++)
            newPool.CreateObject();

        _pools.Add(newPool);

        return true;
    }

    public GameObject GetRestObject(GameObject effectObject)
    {
        ObjectPool pool = FindPool(effectObject);

        return pool.GetObject();
    }

    public GameObject GetRestObject(GameObject effectObject, Transform parent)
    {
        ObjectPool pool = FindPool(effectObject);

        return pool.GetObject(parent, effectObject.transform.position);
    }

    //public ObjectPool FindPool(string prefabName)
    //{
    //    ObjectPool findedPool = null;

    //    foreach (ObjectPool pool in _pools)
    //    {
    //        if (pool.PoolName != prefabName)
    //            continue;

    //        findedPool = pool;
    //        break;
    //    }

    //    return findedPool;
    //}

    public ObjectPool FindPool(GameObject findPrefab)
    {
        foreach (ObjectPool pool in _pools)
        {
            if (pool.pooledObjectPrefab != findPrefab)
                continue;

            return pool;
        }

        CreatePool(findPrefab, 1);

        return FindPool(findPrefab);
    }
}
