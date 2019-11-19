using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private Text _remainCoin;
    [SerializeField]
    private Text _countDown;

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

    //private void OnEnable()
    //{
    //    StartCoroutine(SecondCountdown());
    //}
    //private void OnDisable()
    //{
    //    _time = _maxTime;
    //    StopCoroutine(SecondCountdown());
    //}
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetUIActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetTime(int time)
    {
        _time = time;
        _countDown.text = _time.ToString();
    }
}
