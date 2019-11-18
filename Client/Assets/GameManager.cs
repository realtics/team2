using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CharacterStat _playerStat;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public CharacterMovement player
    {
        get
        {
            return player;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        _playerStat = FindObjectOfType<CharacterStat>().GetComponent<CharacterStat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //To do.
    // GameManager에 코인 사용, 코인을 사용할 수 있는 Countdown과 코인을 전부 사용 후 menu 화면으로 넘어가기.
    public void GameOver()
    {
        if (_playerStat.Hp <= 0)
        {
            UIHelper.Instance.SetGameOver(true);
        }
    }
}
