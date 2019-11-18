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

    private void OnEnable()
    {
        StartCoroutine(SecondCountdown());
    }
    private void OnDisable()
    {
        _time = _maxTime;
        StopCoroutine(SecondCountdown());
    }
    // Use this for initialization
    void Start()
    {
        // 임시 값.
        Coin = 5;

        _time = _maxTime;

    }

    // Update is called once per frame
    void Update()
    {
        CountOver();
    }
    public void UseCoin()
    {
        if(Coin > 0)
        {
            Coin -= 1;
            gameObject.SetActive(false);
        }
    }
    private void CountOver()
    {
        if(_time < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetUIActive(bool active)
    {
        gameObject.SetActive(active);
    }

    IEnumerator SecondCountdown()
    {
        while (true)
        {
            if(_time >= 0)
            {
                _time -= 1;
                _countDown.text = _time.ToString();
            }
            yield return new WaitForSeconds(1);
        }
    }
}
