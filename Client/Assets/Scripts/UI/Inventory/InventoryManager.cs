using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Inventory _inventory;
    [SerializeField]
    private EquipmentPanel _equipmentPanel;

    private void Awake()
    {
        _inventory.OnItemLeftClickEvent += EquipFromInventory;
        _equipmentPanel.OnItemLeftClickEvent += UnequipFromEquipPanel;
    }

    private void EquipFromInventory(Item item)
    {
        if(item is EquipableItem)
        {
            Equip((EquipableItem)item);
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        if (item is EquipableItem)
        {
            Unequip((EquipableItem)item);
        }
    }

    public void Equip(EquipableItem item)
    {
        if(_inventory.RemoveItem(item))
        {
            EquipableItem previousItem;
            if(_equipmentPanel.AddItem(item, out previousItem))
            {
                if(previousItem != null)
                {
                    _inventory.AddItem(previousItem);
                }
            }
            else
            {
                _inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquipableItem item)
    {
        if(!_inventory.IsFull() && _equipmentPanel.RemonveItem(item))
        {
            _inventory.AddItem(item);
        }
    }
}
