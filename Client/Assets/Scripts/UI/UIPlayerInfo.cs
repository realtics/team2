using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerInfo : MonoBehaviour
{
    [SerializeField]
    private UIPlayerStat _hpBar;
    [SerializeField]
    private UIPlayerStat _mpBar;
    [SerializeField]
    private CharacterStat _characterStat;

    // Use this for initialization
    void Start()
    {
        if (_characterStat != null)
        {
            _hpBar.SetStat(_characterStat.Hp, _characterStat.MaxHp);
            _mpBar.SetStat(_characterStat.Mp, _characterStat.MaxMp);
        }
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
