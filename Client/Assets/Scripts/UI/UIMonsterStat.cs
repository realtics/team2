using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMonsterStat : UIStat
{
    private int _multple;
    private const int _divideHp = 100;


    [SerializeField]
    private Text _TextHpMultple;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HpMultpleProcess();
    }


    //ToDo. 데미지 입을시 데미지만큼 흰색바를 만든후 0.5초 뒤에 흰색바를 지움.
    private void HpWhiteRed()
    {


    }

    private void HpMultpleProcess()
    {   
        if (_currentValue > 0)
        {
            _multple = (int)(_currentValue / _divideHp);
            if(_multple > 0)
                _TextHpMultple.text = "x" + _multple;
            else
                _TextHpMultple.text = "";
        }
        else
        {
            return;
        }
    }
    public void ResetStat()
    {
        _content.fillAmount = 1.0f;
    }
    public bool IsDie()
    {
        if (CurrentValue <= 0)
        {
            ResetStat();
            return true;
        }
        return false;
    }
}
