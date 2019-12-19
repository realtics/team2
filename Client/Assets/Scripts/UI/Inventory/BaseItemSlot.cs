using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseItemSlot : MonoBehaviour
{
	[SerializeField] 
	protected Image _image;
	[SerializeField]
	protected ItemToolTip _itemToolTip;

	public event Action<BaseItemSlot> OnClickEvent;

	protected Color normalColor = Color.white;
	protected Color disabledColor = new Color(1, 1, 1, 0);
	[SerializeField]
	protected Item _item;
	public Item Item
	{
		get { return _item; }
		set
		{
			_item = value;
			if (_item == null && Amount != 0) Amount = 0;

			if (_item == null)
			{
				_image.sprite = null;
				_image.color = disabledColor;
			}
			else
			{
				_image.sprite = _item.Icon;
				_image.color = normalColor;
			}

			if (isPointerOver)
			{
				OnPointerExit(null);
				OnPointerEnter(null);
			}
		}
	}

	private int _amount;
	public int Amount
	{
		get { return _amount; }
		set
		{
			_amount = value;
			if (_amount < 0) _amount = 0;
			if (_amount == 0 && Item != null) Item = null;

			if (amountText != null)
			{
				amountText.enabled = _item != null && _amount > 1;
				if (amountText.enabled)
				{
					amountText.text = _amount.ToString();
				}
			}
		}
	}

	public virtual bool CanAddStack(Item item, int amount = 1)
	{
		return Item != null && Item.ID == item.ID;
	}

	public virtual bool CanReceiveItem(Item item)
	{
		return false;
	}

	protected virtual void OnValidate()
	{
		if (_image == null)
			_image = GetComponent<Image>();

		if (amountText == null)
			amountText = GetComponentInChildren<Text>();

		Item = _item;
		Amount = _amount;
	}

	protected virtual void OnDisable()
	{
		if (isPointerOver)
		{
			OnPointerExit(null);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
		{
			if (OnClickEvent != null)
				OnClickEvent(this);
		}
	}
}
