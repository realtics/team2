using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerInfo : MonoBehaviour
{
    private PlayerStat _hpBar;
    private PlayerStat _mpBar;
    private Text _level;

    // temp value.
    private int _hp = 100; 
    private int _Mp = 50;

    void Awake()
    {
        _hpBar = transform.Find("Hp").transform.Find("PlayerHpBar").GetComponent<PlayerStat>();
        _mpBar = transform.Find("Mp").transform.Find("PlayerMpBar").GetComponent<PlayerStat>();
    }

    // Use this for initialization
    void Start()
    { 
        _hpBar.Initialize(_hp, _hp);
        _mpBar.Initialize(_Mp, _Mp);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddHp(float value)
    {
        _hpBar.CurrentValue += value;
    }
    public void AddMp(float value)
    {
        _mpBar.CurrentValue += value;
    }

}
