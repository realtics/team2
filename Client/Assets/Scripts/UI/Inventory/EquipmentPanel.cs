using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField]
    private Transform _equipmentSlotsParent;
    [SerializeField]
    private EquipmentSlot[] _equipmentSlots;

    public event Action<Item> OnItemLeftClickEvent;

    private void Start()
    {
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            _equipmentSlots[i].OnRightClickEvent += OnItemLeftClickEvent;
        }
    }

    private void OnValidate()
    {
        _equipmentSlots = _equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquipableItem item, out EquipableItem previousItem)
    {
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            if (_equipmentSlots[i].equipmentType == item.equipmentType)
            {
                previousItem = (EquipableItem)_equipmentSlots[i].Item;
                _equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemonveItem(EquipableItem item)
    {
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            if (_equipmentSlots[i].Item == item)
            {
                _equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}
