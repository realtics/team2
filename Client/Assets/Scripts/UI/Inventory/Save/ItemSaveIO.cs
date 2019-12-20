﻿using UnityEngine;

public static class ItemSaveIO
{
	private static readonly string baseSavePath;

	static ItemSaveIO()
	{
		baseSavePath = Application.persistentDataPath;
	}

	public static void SaveItems(ItemContainerSaveData items, string path)
	{
		FileReadWrite.WriteToBinaryFile(baseSavePath + "/" + path + ".dat", items);
	}

	public static ItemContainerSaveData LoadItems(string path)
	{
		string filePath = baseSavePath + "/" + path + ".dat";

		if(System.IO.File.Exists(filePath))
		{
			return FileReadWrite.ReadFromBinaryFile<ItemContainerSaveData>(filePath);
		}
		return null;
	}

	public static void SaveResultItem(string ID)
	{
		ItemContainerSaveData savdData = ItemSaveIO.LoadItems("Inventory");

		for (int i = 0; i < savdData.SavedSlots.Length; i++)
		{
			if (savdData.SavedSlots[i] == null)
			{
				savdData.SavedSlots[i] = new ItemSlotSaveData(ID);
				ItemSaveIO.SaveItems(savdData, "Inventory");
				return;
			}
		}
	}
}
