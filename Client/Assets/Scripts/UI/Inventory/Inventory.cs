using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour, IItemContainer
{
	//[FormerlySerializedAs("_startingItems")]
	[SerializeField]
    private List<Item> _items;
    [SerializeField]
    private Transform _itemsParent;
    [SerializeField]
    private ItemSlot[] _itemSlots;

    public event Action<Item> OnItemClickEvent;

    private void Start()
    {
        for(int i =0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].OnClickEvent += OnItemClickEvent;
        }

		RefreshUI();
	}

    private void OnValidate()
    {
        if (_itemsParent != null)
            _itemSlots = _itemsParent.GetComponentsInChildren<ItemSlot>();

        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for(; i < _items.Count && i < _itemSlots.Length; i++)
        {
			//TODO : 확인
            //_itemSlots[i].Item = Instantiate(_items[i]);
			_itemSlots[i].Item = _items[i];
		}

        for(; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item)
    {
		if (IsFull())
			return false;

		_items.Add(item);
		RefreshUI();
		return true;

		//for(int i=0; i < _itemSlots.Length; i++)
		//{
		//	if(_itemSlots[i].Item == null)
		//	{
		//		_itemSlots[i].Item = item;
		//		return true;
		//	}
		//}
		//return false;
	}

    public bool RemoveItem(Item item)
    {
		if (_items.Remove(item))
		{
			RefreshUI();
			return true;
		}
		return false;
		//for (int i = 0; i < _itemSlots.Length; i++)
		//{
		//	if (_itemSlots[i].Item == item)
		//	{
		//		_itemSlots[i].Item = item;
		//		return true;
		//	}
		//}
		//return false;
	}

	public Item RemoveItem(string itemID)
	{
		for(int i =0; i < _itemSlots.Length; i++)
		{
			Item item = _itemSlots[i].Item;
			if(item != null && item.ID == itemID)
			{
				_itemSlots[i].Item = null;
				return item;
			}
		}
		return null;
	}
	

    //Fixme : 속성으로 만들기
    public bool IsFull()
    {
		return _items.Count >= _itemSlots.Length;

		//for (int i = 0; i < _itemSlots.Length; i++)
		//{
		//	if (_itemSlots[i].Item == null)
		//	{
		//		return false;
		//	}
		//}
		//return true;
	}

	public void Clear()
	{
		for (int i = 0; i < _itemSlots.Count; i++)
		{
			if (ItemSlots[i].Item != null && Application.isPlaying)
			{
				ItemSlots[i].Item.Destroy();
			}
			ItemSlots[i].Item = null;
			ItemSlots[i].Amount = 0;
		}
	}
}
