using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerInfo : MonoBehaviour
{
    [SerializeField]
    private UIPlayerStat _hpBar;
    [SerializeField]
    private UIPlayerStat _mpBar;
    private CharacterStat characterStat;

    // Use this for initialization
    void Start()
    {
        characterStat = FindObjectOfType<CharacterStat>();
        _hpBar.SetStat(characterStat.Hp, characterStat.MaxHp);
        _mpBar.SetStat(characterStat.Mp, characterStat.MaxMp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerHp(float value, float maxValue)
    {
        _hpBar.SetStat(value, maxValue);
    }
    public void SetPlayerMp(float value, float maxValue)
    {
        _mpBar.SetStat(value, maxValue);
    }
}
