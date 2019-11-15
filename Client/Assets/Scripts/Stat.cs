using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stat : MonoBehaviour
{
    private Image _content;

    [SerializeField]
    private float _lerpSpeed = 3;

    protected float _currentFill;
    public float MaxValue { get { return _maxValue; } set { _maxValue = value; } }

    protected float _maxValue;
    protected float _currentValue;
    public virtual float CurrentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            if (value > _maxValue)
                _currentValue = _maxValue;
            else if (value < 0)
                _currentValue = 0;
            else
                _currentValue = value;

            _currentFill = _currentValue / _maxValue;
        }
    }


    // Use this for initialization
    protected virtual void Start()
    {
        _content = GetComponent<Image>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(_currentFill != _content.fillAmount)
        {
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, _currentFill, Time.deltaTime * _lerpSpeed);
        }
    }
    public void Initialize(float currentValue,float maxValue)
    {
        MaxValue = maxValue;
        CurrentValue = currentValue;
    }

}
