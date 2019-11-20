using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResult : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _resultBox;
    [SerializeField]
    private Image _openResultBox;
    [SerializeField]
    private Text _countDown;

    private Image[] _resultBoxImage;
    private Button[] _resultBoxButton;

    // Start is called before the first frame update
    void Start()
    {
        _resultBoxImage = new Image[_resultBox.Length];
        _resultBoxButton = new Button[_resultBox.Length];


        for (int i = 0; i < _resultBox.Length; i++)
        {
            int localCopy = i;
            _resultBoxImage[i] = _resultBox[i].GetComponent<Image>();
            _resultBoxButton[i] = _resultBox[i].GetComponent<Button>();
            _resultBoxButton[i].onClick.AddListener(() => { GameManager.Instance.OnClickResultBox(localCopy); });
        }
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
    public void OpenResultBox(int index)
    {
        _resultBoxImage[index].sprite = _openResultBox.sprite;
    }
}
