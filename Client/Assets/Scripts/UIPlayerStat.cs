using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerStat : UIStat
{
    [SerializeField]
    private Text _textValue;

    public override float CurrentValue
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
            _textValue.text = _currentValue + "/" + _maxValue;
        }
    }

    // Use this for initialization
    protected override void  Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void  Update()
    {
        base.Update();
    }
}
