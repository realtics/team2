﻿using System;
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

	public ItemSlot[] ItemSlots { get { return _itemSlots;} }

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
	}

    public bool RemoveItem(Item item)
    {
		if (_items.Remove(item))
		{
			RefreshUI();
			return true;
		}
		return false;
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
	}

	public void Clear()
	{
		for (int i = 0; i < _itemSlots.Length; i++)
		{
			if (_itemSlots[i].Item != null && Application.isPlaying)
			{
				_itemSlots[i].Item.Destroy();
			}
			_itemSlots[i].Item = null;
		}
	}
}
