using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private Text _remainCoin;
    [SerializeField]
    private Image _countDown;
    [SerializeField]
    private Sprite[] _countDownSprite;

    private int _coin;
    private int _time;

    private const int _maxTime = 10;

    public int Coin
    {
        get
        {
            return _coin;
        }
        set
        {
            _coin = value;
            _remainCoin.text = _coin.ToString();
        }
    }

    public void SetUIActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetTime(int time)
    {
        _time = time;
        if (_time != _maxTime)
            _countDown.sprite = _countDownSprite[_time];
    }
}
