using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private GameObject _originalObject;
    private List<T> _spawnedObjects;

    private void Awake()
    {
        _spawnedObjects = new List<T>();
    }
    public T GetObject()
    {
        T obj = FindRestObject();

        if (obj == null)
        {
            Debug.LogError("오브젝트 불러오기 실패!");
            return null;
        }

        obj.gameObject.SetActive(true);

        return obj;
    }

    public T GetObject(Vector3 position)
    {
        T obj = GetObject();

        if (obj == null)
            return null;

        obj.transform.position = position;

        return obj;
    }

    public T GetObject(Transform parent, Vector3 position)
    {
        T obj = GetObject();

        if (obj == null)
            return null;

        obj.transform.SetParent(parent);
        obj.transform.position = position;

        return obj;
    }

    private T CreateObject()
    {
        if (_originalObject == null)
        {
            Debug.LogError("생성 할 프리팹이 없습니다.");
            return null;
        }

        GameObject newObject = Instantiate(_originalObject);
        T obj = newObject.GetComponent<T>();

        if (obj == null)
        {
            Debug.LogError("생성 된 프리팹에 " + obj.ToString() + "이 없습니다.");
            Destroy(newObject);
            return null;
        }

        _spawnedObjects.Add(obj);

        return obj;
    }

    private T FindRestObject()
    {
        foreach (T obj in _spawnedObjects)
        {
            if (obj.gameObject.activeInHierarchy)
                continue;

            return obj;
        }

        return CreateObject();
    }
}
