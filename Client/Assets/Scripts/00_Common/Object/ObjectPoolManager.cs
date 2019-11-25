using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ObjectPoolType
{
    Jingongcham = 1,
}

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> _pools;

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
        _pools = new List<ObjectPool>();
    }

    private void Start()
    {
        
    }

    public bool CreatePool(ObjectPoolType poolType, GameObject prefab)
    {
        foreach (ObjectPool pool in _pools)
        {
            if (pool.poolType == poolType)
                return false;
        }

        ObjectPool newPool = new ObjectPool();

        newPool.CreatePool(poolType, prefab);
        _pools.Add(newPool);

        return true;
    }

    public GameObject GetRestObject(ObjectPoolType type)
    {
        ObjectPool pool = FindPool(type);

        return pool.GetObject();
    }

    private ObjectPool FindPool(ObjectPoolType type)
    {
        ObjectPool findedPool = null;

        foreach (ObjectPool pool in _pools)
        {
            if (pool.poolType != type)
                continue;

            findedPool = pool;
            break;
        }

        return findedPool;
    }
}
