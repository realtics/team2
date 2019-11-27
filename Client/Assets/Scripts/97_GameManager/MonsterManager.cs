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
    [SerializeField]
    private List<BaseMonster> monsterList = new List<BaseMonster>();
    [SerializeField]
    private int _monsterCount;

    void Awake()
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
        monsterList.Add(spawnMonster.GetComponent<BaseMonster>());
    }

    public void ClearMonsterList()
    {
        monsterList.Clear();
    }

    public void ReceiveMonsterDie(BaseMonster monster)
    {

        monsterList.Remove(monster);
    }
}
