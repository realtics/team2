using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public struct MonsterInfo
{
    public string name;
    public int level;
    public float currentHp;
}

public class UIMosterInfo : MonoBehaviour
{
    private UIMonsterStat _hpBar;
    private Text _name;
    private Text _level;
    private Text _multiple;

    // temp value.
    private int _hp = 200;

    void Awake()
    {
        _hpBar = transform.Find("Hp").transform.Find("MosterHpBar").GetComponent<UIMonsterStat>();
        _name = transform.Find("Name").transform.Find("MosterName").GetComponent<Text>();
        _level = transform.Find("Level").transform.Find("MosterLevel").GetComponent<Text>();
        _multiple = transform.Find("Hp").transform.Find("HpMultple").GetComponent<Text>();
    }

    void Start()
    {
        // Hack.. 테스트용.몬스터 정보 받아오면 지울 예정.,
        _hpBar.SetStat(_hp, _hp);
    }

    void Update()
    {
        CheckDieMonster();
    }
    public void SetHp(float CurrentHp, float MaxHp)
    {
        _hpBar.CurrentValue = CurrentHp;
        _hpBar.MaxValue = MaxHp;
    }
    // ToDo.
    // MonsterInfo 말고 GameObject로 받아서 처리 할 예정, Monster GameObject가 만들어 질 때 까지 대기.
    public void SetMonster(MonsterInfo info)
    {
        _name.text = info.name;
        _level.text = "Lv." + info.level;
        _hpBar.CurrentValue = info.currentHp;
    }
    public void CheckDieMonster()
    {
        if(_hpBar.IsDie())
        {
            gameObject.SetActive(false);
        }
    }
}