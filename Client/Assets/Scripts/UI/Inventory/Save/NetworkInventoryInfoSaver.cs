using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInventoryInfoSaver
{
	private NetworkInventoryInfoSaver() { }
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

	private List<string> _itemIDs;
	public List<string> ItemIDs { get { return _itemIDs; } }

	public void SaveItemIDs(ItemSlot[] itemSlots)
	{
		_itemIDs.Clear();
		for(int i=0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i].Item == null)
				return;

			_itemIDs.Add(itemSlots[i].Item.NetID);
		}
	}

	public void SaveItemIDs(List<string> itemIds)
	{
		_itemIDs.Clear();
		_itemIDs = itemIds;
	}

}
