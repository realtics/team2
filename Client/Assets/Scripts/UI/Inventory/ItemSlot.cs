using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private ItemToolTip _itemToolTip;

	private Button _button;

    public event Action<Item> OnRightClickEvent;

    private Item _item;
    public Item Item
    { get { return _item; }
      set
        {
            _item = value;

            if (_item == null)
                _image.enabled = false;
            else
            {
                _image.sprite = _item.icon;
                _image.enabled = true;
            }
        } 
    }
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(ClickSlot);
	}

	protected virtual void OnValidate()
    {
        if (_image == null)
            _image = GetComponent<Image>();

        //게임실행중이면 느릴거지만, 어차피 에디터에서 찾고 게임중엔 찾지않을 것이 분명
        if (_itemToolTip == null)
            _itemToolTip = FindObjectOfType<ItemToolTip>();
    }

	public void OnPointerClick(PointerEventData eventData)
    {
       if(eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
                OnRightClickEvent(Item);
        }
	}
	
    public void OnPointerEnter(PointerEventData eventData)
    {
		

	}

    public void OnPointerExit(PointerEventData eventData)
    {
    }

	public void ClickSlot()
	{
		if (Item is EquipableItem)
		{

			if(_itemToolTip.enabled)
				_itemToolTip.HideToolTip();

			_itemToolTip.ShowToolTip((EquipableItem)Item, this);
		}
	}

	public void ClickEvent()
	{
		OnRightClickEvent(Item);
	}
}
