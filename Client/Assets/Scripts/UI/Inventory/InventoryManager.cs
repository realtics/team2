using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	private static InventoryManager _instance;
	public static InventoryManager Instance
	{
		get
		{
			return _instance;
		}
	}

	[SerializeField]
	private Inventory _inventory;
	[SerializeField]
	private EquipmentPanel _equipmentPanel;
	[SerializeField]
	private ItemToolTip _inventoryTooltip;
	[SerializeField]
	private ItemToolTip _equipPanelTooltip;

	public ItemToolTip InventoryTooltip{ get { return _inventoryTooltip; } }
	public ItemToolTip EquipPanelTooltip { get { return _equipPanelTooltip; } }
	public Inventory Inventory { get { return _inventory; } }

	private void Awake()
    {
		_instance = this;
		_inventory.OnItemClickEvent += EquipFromInventory;
        _equipmentPanel.OnItemClickEvent += UnequipFromEquipPanel;
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
