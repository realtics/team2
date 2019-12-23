using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    protected ItemToolTip _itemToolTip;
	private Button _button;
    public event Action<Item> OnClickEvent;

	[SerializeField]
    private Item _item;
    public Item Item
    { get { return _item; }
      set
        {
            _item = value;

            if (_item == null)
			{
				_image.enabled = false;
				_image.sprite = null;
			}
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

		FindToolTip();
	}

	protected void FindToolTip()
	{
		if (_itemToolTip == null)
			_itemToolTip = InventoryManager.Instance.ItemTooltip;
	}

	public void ClickSlot()
	{
		if (_item is EquipableItem)
		{

			if(_itemToolTip.enabled)
				_itemToolTip.HideToolTip();

			_itemToolTip.ShowToolTip((EquipableItem)_item, this);
		}
	}

	public void ClickSellButton()
	{
		InventoryManager.Instance.Inventory.RemoveItem(_item);
	}

	public void ClickEvent()
	{
		OnClickEvent(_item);
	}
}
