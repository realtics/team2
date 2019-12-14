using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Item> _items;
    [SerializeField]
    private Transform _itemsParent;
    [SerializeField]
    private ItemSlot[] _itemSlots;

    public event Action<Item> OnItemRightClickEvent;

    private void Awake()
    {
        for(int i =0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].OnRightClickEvent += OnItemRightClickEvent;
        }
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
        if(_items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }

    //Fixme : 속성으로 만들기
    public bool IsFull()
    {
        return _items.Count >= _itemSlots.Length;
    }
}
