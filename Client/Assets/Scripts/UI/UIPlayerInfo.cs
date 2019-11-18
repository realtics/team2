using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerInfo : MonoBehaviour
{
    private UIPlayerStat _hpBar;
    private UIPlayerStat _mpBar;
    private Text _level;

    void Awake()
    {
        _hpBar = transform.Find("Hp").transform.Find("PlayerHpBar").GetComponent<UIPlayerStat>();
        _mpBar = transform.Find("Mp").transform.Find("PlayerMpBar").GetComponent<UIPlayerStat>();

    }

    // Use this for initialization
    void Start()
    {
        CharacterStat characterStat = FindObjectOfType<CharacterStat>();
        _hpBar.Initialize(characterStat.Hp, characterStat.MaxHp);
        _mpBar.Initialize(characterStat.Mp, characterStat.MaxMp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
