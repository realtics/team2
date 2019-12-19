using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
	private const string InventoryFileName = "Inventory";
	private const string EquipmentFilename = "Equipment";

	//public void LoadInventory()
	//{
	//	ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(InventoryFileName);
	//	if (savedSlots == null) return;
	//	InventoryManager.Instance.Inventory.

	//	for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
	//	{
	//		ItemSlot itemSlot = character.Inventory.ItemSlots[i];
	//		ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

	//		if (savedSlot == null)
	//		{
	//			itemSlot.Item = null;
	//		}
	//		else
	//		{
	//			itemSlot.Item = itemDatabase.GetItemCopy(savedSlot.ItemID);
	//		}
	//	}
	//}

	private void SaveItems(IList<ItemSlot> itemSlots, string fileName)
	{
		var saveData = new ItemContainerSaveData(itemSlots.Count);

		for(int i=0; i < saveData.SavedSlots.Length; i++)
		{
			ItemSlot itemSlot = itemSlots[i];

			if(itemSlot.Item == null)
			{
				saveData.SavedSlots[i] = null;
			}
			else
			{
				saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID);
			}
		}

		ItemSaveIO.SaveItems(saveData, fileName);

	}
}
