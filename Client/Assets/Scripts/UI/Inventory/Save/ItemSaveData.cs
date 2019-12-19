using System;

[Serializable]
public class ItemSlotSaveData
{
	public string ItemID;
	
	public ItemSlotSaveData(string id)
	{
		ItemID = id;
	}
}

[Serializable]
public class ItemContainerSaveData
{
	public ItemSlotSaveData[] SavedSlots;

	public ItemContainerSaveData(int numItems)
	{
		SavedSlots = new ItemSlotSaveData[numItems];
	}
}