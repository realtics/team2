using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResult : MonoBehaviour
{
    [SerializeField]
    private Image[] _resultBox;
    [SerializeField]
    private Image _openResultBox;
    [SerializeField]
    private Text _countDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTime(int time)
    {
        _countDown.text = time.ToString();
    }
    // ToDo. 클릭시에 GameManager에서 UI를 호출해서 Sprite 변경 및 임시 아이템 띄우기.
    public void OnClickResultBox(int index)
    {
        _resultBox[index].sprite = _openResultBox.sprite;
    }
    public void AllOpenResultBox()
    {
        for(int i = 0; i < _resultBox.Length; i++)
        {
            _resultBox[i].sprite = _openResultBox.sprite;
        }
    }
}
