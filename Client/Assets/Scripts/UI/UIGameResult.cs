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
    private Image _countDown;
    [SerializeField]
    private Sprite[] _countImage;
	[SerializeField]
	private Image _itemIcon;
	[SerializeField]
	private Text _itemName;
	[SerializeField]
	private ResultItemDatabase _itemDatabase;

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
            _resultBoxButton[i].onClick.AddListener(() => { DungeonGameManager.Instance.OnClickResultBox(localCopy); });
        }
    }

    public void SetTime(int time)
    {
        if (time > 3)
            time = 3;
        _countDown.sprite = _countImage[time];
    }
    // ToDo. 클릭시에 GameManager에서 UI를 호출해서 Sprite 변경 및 임시 아이템 띄우기.
    public void OpenResultBox(int index)
    {
		
		if (NetworkManager.Instance.IsConnect)
		{
			ItemSaveIO.SaveResultItem(NetworkManager.Instance.ItemId);
			Item newItem = _itemDatabase.GetItemReference(NetworkManager.Instance.ItemId);

			_itemIcon.sprite = newItem.icon;
			_itemName.text = newItem.itemName;

			Debug.Log(NetworkManager.Instance.ItemIndex+"의 아이템을 서버로부터 받았습니다.");
		}
			//아이템얻기 테스트 잘된당!!~~!! 이히~~!!!!
		else
		{
			Sprite icon;
			string itemName;
			ItemSaveIO.SaveResultItem(_itemDatabase.GetRandomItemID(out icon, out itemName));
			_itemIcon.sprite = icon;
			_itemName.text = itemName;
		}
		_openResultBox.transform.position = _resultBox[index].transform.position;

	}
}
