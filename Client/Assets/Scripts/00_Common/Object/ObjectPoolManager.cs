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

    public bool CreatePool(GameObject prefab)
    {
        foreach (ObjectPool pool in _pools)
        {
            if (pool.PoolName == prefab.name)
                return false;
        }

        ObjectPool newPool = new ObjectPool();

        newPool.CreatePool(prefab);
        _pools.Add(newPool);

        return true;
    }

    public GameObject GetRestObject(GameObject effectObject)
    {
        ObjectPool pool = FindPool(effectObject.name);

        return pool.GetObject();
    }

    public ObjectPool FindPool(string prefabName)
    {
        ObjectPool findedPool = null;

        foreach (ObjectPool pool in _pools)
        {
            if (pool.PoolName != prefabName)
                continue;

            findedPool = pool;
            break;
        }

        return findedPool;
    }
}
