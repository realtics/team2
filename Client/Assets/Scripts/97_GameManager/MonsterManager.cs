using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _instance;
    public static MonsterManager Instance
    {
        get
        {
            return _instance;
        }
    }
    [SerializeField]
    private List<BaseMonster> _monsterList = new List<BaseMonster>();

    void Awake()
    {
        _instance = this;
    }

    public void AddMonster(GameObject prefab, Vector3 position)
    {
        GameObject spawnMonster = ObjectPoolManager.Instance.GetRestObject(prefab);
        spawnMonster.transform.position = position;
        _monsterList.Add(spawnMonster.GetComponent<BaseMonster>());
    }

    public bool IsExistMonster()
    {
        if (_monsterList.Count > 0)
            return true;

        return false;
    }

    public void ReceiveMonsterDie(BaseMonster monster)
    {
        monster.InactiveMonster();
        monster.ResetMonster();
        _monsterList.Remove(monster);

        //FIXME : 포탈을 여는 주체가 몬스터매니저가 할일인가
        if (_monsterList.Count == 0)
            PotalManager.instance.ResetPotals();
    }
}
