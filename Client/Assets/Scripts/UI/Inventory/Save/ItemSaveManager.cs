using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
    [SerializeField] ItemDatabase itemDatabase;

    private const string InventoryFileName = "Inventory";
    private const string EquipmentFileName = "Equipment";

    public void LoadInventory()
    {
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(InventoryFileName);
        if (savedSlots == null) return;
        InventoryManager.Instance.Inventory.Clear();

        for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = InventoryManager.Instance.Inventory.ItemSlots[i];
            ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

            if (savedSlot == null)
            {
                itemSlot.Item = null;
            }
            else
            {
                //itemSlot.Item = itemDatabase.GetItemCopy(savedSlot.ItemID);
                InventoryManager.Instance.Inventory.AddItem(itemDatabase.GetItemCopy(savedSlot.ItemID));
            }
        }
    }

    public void LoadEquipment()
    {
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(EquipmentFileName);
        if (savedSlots == null) return;

        foreach (ItemSlotSaveData savedSlot in savedSlots.SavedSlots)
        {
            if (savedSlot == null)
            {
                continue;
            }

            Item item = itemDatabase.GetItemCopy(savedSlot.ItemID);
            InventoryManager.Instance.Inventory.AddItem(item);
            InventoryManager.Instance.Equip((EquipableItem)item);
        }
    }
    public void SaveInventory()
    {
        SaveItems(InventoryManager.Instance.Inventory.ItemSlots, InventoryFileName);
    }

    public void SaveEquipment()
    {
        SaveItems(InventoryManager.Instance.EquipmentPanel.EquipmentSlots, EquipmentFileName);
    }

    private void SaveItems(IList<ItemSlot> itemSlots, string fileName)
    {
        var saveData = new ItemContainerSaveData(itemSlots.Count);

        for (int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = itemSlots[i];

            if (itemSlot.Item == null)
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

    public void LoadItemsByNetID()
    {
        //LoadEquipment
        foreach (string ID in NetworkInventoryInfoSaver.Instance.EquipIDs)
        {
            if (ID == "0")
            {
                continue;
            }

            Item item = itemDatabase.GetItemCopyByNetId(ID);
            InventoryManager.Instance.Inventory.AddItem(item);
            InventoryManager.Instance.Equip((EquipableItem)item);
        }
        //LoadInventory
        if (NetworkInventoryInfoSaver.Instance.InventoryIDs != null)
        foreach (string ID in NetworkInventoryInfoSaver.Instance.InventoryIDs)
        {
            InventoryManager.Instance.Inventory.AddItem(itemDatabase.GetItemCopyByNetId(ID));
        }
    }
}
