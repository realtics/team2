using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public struct MonsterInfo
{
    public Monster.MonsterTypeInfo index;
    public string name;
    public int level;
    public float currentHp;
    public float maxHp;
}

public class UIMosterInfo : MonoBehaviour
{
    [SerializeField]
    private UIMonsterStat _hpBar;
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _level;
    [SerializeField]
    private Text _multiple;
    [SerializeField]
    private Image _snapShot;

    [SerializeField]
    private Sprite[] _mosterSnapShot;

    void Start()
    {

    }

    void Update()
    {
        CheckDieMonster();
    }
    public void SetHp(float CurrentHp, float MaxHp)
    {
        _hpBar.MaxValue = MaxHp;
        _hpBar.CurrentValue = CurrentHp;
    }

    public void SetMonster(MonsterInfo info)
    {
        _name.text = info.name;
        _level.text = "Lv." + info.level;
        SetHp(info.currentHp, info.maxHp);
        _snapShot.sprite = _mosterSnapShot[(int)info.index];
    }
    public void CheckDieMonster()
    {
        if(_hpBar.IsDie())
        {
            gameObject.SetActive(false);
        }
    }
}