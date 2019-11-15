using UnityEngine;
using System.Collections;

public class UIHelper : MonoBehaviour
{
    private static UIHelper _instance;
    public static UIHelper Instance
    {
        get
        {
            return _instance;       
        }
    }

    private UIPlayerInfo _playerInfo;
    private UIMosterInfo _MosterInfo;

    // Use this for initialization
    void Start()
    {
        _instance = this;

        _playerInfo = GameObject.Find("PlayerInfo").GetComponent<UIPlayerInfo>();
        _MosterInfo = GameObject.Find("MonsterInfo").GetComponent<UIMosterInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stat Test.
        if(Input.GetKeyDown(KeyCode.Z))
        {
            AddPlayerHp(-10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddPlayerMp(-10);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddMonsterHp(-10);
        }
    }
    public void AddPlayerHp(float value)
    {
        _playerInfo.AddHp(value);
    }
    public void AddPlayerMp(float value)
    {
        _playerInfo.AddMp(value);
    }
    public void AddMonsterHp(float value)
    {
        _MosterInfo.AddHp(value);
    }
}
