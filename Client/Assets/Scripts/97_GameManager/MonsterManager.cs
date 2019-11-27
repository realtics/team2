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

    public bool IsExistMonster()
    {
        if (monsterList.Count > 0)
            return true;
        else
            return false;
    }

    public void ReceiveMonsterDie(BaseMonster monster)
    {
        monster.InactiveMonster();
        monster.ResetMonster();
        monsterList.Remove(monster);

        //FIXME : 포탈을 여는 주체가 몬스터매니저가 할일인가
        if (monsterList.Count == 0)
            PotalManager.instance.ResetPotals();
    }
}
