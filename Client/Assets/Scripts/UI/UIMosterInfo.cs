using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIMosterInfo : MonoBehaviour
{
    private Stat _hpBar;
    private Text _name;
    private Text _level;
    private Text _multiple;

    // temp value.
    private int _hp = 100;

    void Start()
    {
        _hpBar = transform.Find("Hp").transform.Find("MosterHpBar").GetComponent<Stat>();
        _name = transform.Find("Name").transform.Find("MosterName").GetComponent<Text>();
        _level = transform.Find("Level").transform.Find("MosterLevel").GetComponent<Text>();
        _multiple = transform.Find("HpMultple").transform.Find("MonsterMultple").GetComponent<Text>();

        _hpBar.Initialize(_hp, _hp);
    }

    void Update()
    {
        
    }
    public void AddHp(float value)
    {
        _hpBar.CurrentValue += value;
    }

}