using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _Instance;
    public static MonsterManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    public List<GameObject> monster = new List<GameObject>();
    public List<GameObject> deleteMonster = new List<GameObject>();

    private int _monsterCount;

    void Start()
    {
        _Instance = this;
    }

    void Update()
    {

    }

    public void AddMonster(GameObject obj, Vector3 position)
    {
        GameObject spawnMonster = ObjectPoolManager.Instance.GetRestObject(obj);
        spawnMonster.transform.position = position;
       
    }

    public void Instantiate(int index)
    {
        ////_monsterCount
        //for()

    }
    public void RoomMonsterDestroy(int Index)
    {

    }

    public void ReceiveMonsterDie()
    {

    }
}
