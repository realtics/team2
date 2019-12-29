using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInventoryInfoSaver
{
	private NetworkInventoryInfoSaver()
	{
		_inventoryIDs = new List<string>();
		_equipIDs = new List<string>();
	}
	private static NetworkInventoryInfoSaver _instance;
	public static NetworkInventoryInfoSaver Instance
	{
		get
		{
			if (_instance == null)
				_instance = new NetworkInventoryInfoSaver();

			return _instance;
		}
	}

    private string _itemID;
    private List<string> _inventoryIDs;
	private List<string> _equipIDs;

    private bool _RES_INVENTORY_OPEN = false;

    public string ItemID { get { return _itemID; } set { _itemID = value; } }
    public List<string> InventoryIDs { get { return _inventoryIDs; } }
	public List<string> EquipIDs { get { return _equipIDs; } }
    public bool RES_INVENTORY_OPEN { get { return _RES_INVENTORY_OPEN; } set { _RES_INVENTORY_OPEN = value; } }

    public bool InventoryInitValue = false;

    public void SaveItemIDs(ItemSlot[] equipSlots, ItemSlot[] inventorySlots)
	{
		_equipIDs.Clear();
		_inventoryIDs.Clear();

		for (int i = 0; i < equipSlots.Length; i++)
		{
			if (equipSlots[i].Item == null)
				_equipIDs.Add("0");
			else
				_equipIDs.Add(equipSlots[i].Item.NetID);
		}

		for (int i = 0; i < inventorySlots.Length; i++)
		{
			if (inventorySlots[i].Item == null)
				return;

			_inventoryIDs.Add(inventorySlots[i].Item.NetID);
		}
	}

	public void SaveItemIDs(List<string> equipIDs, List<string> inventoryIDs)
	{
		_equipIDs.Clear();
		_inventoryIDs.Clear();

        if(equipIDs != null)
            _equipIDs = equipIDs;

        if(inventoryIDs != null)
            _inventoryIDs = inventoryIDs;
	}

}
