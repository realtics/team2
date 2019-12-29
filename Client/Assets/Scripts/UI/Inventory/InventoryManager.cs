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
    private ItemToolTip _itemTooltip;
    [SerializeField]
    private ItemSaveManager _itemSaveManager;
    [SerializeField]
    private ChracterStatInfo _chracterStatInfo;

    public ItemToolTip ItemTooltip { get { return _itemTooltip; } }
    public Inventory Inventory { get { return _inventory; } }
    public EquipmentPanel EquipmentPanel { get { return _equipmentPanel; } }

    private void Awake()
    {
        _instance = this;
        _inventory.OnItemClickEvent += EquipFromInventory;
        _equipmentPanel.OnItemClickEvent += UnequipFromEquipPanel;
    }

    private void Start()
    {
        if (NetworkManager.Instance.IsSingle)
        {
            Load();
        }
           
        else if(!NetworkManager.Instance.IsSingle)
        {
            LoadMulti();

            if (NetworkInventoryInfoSaver.Instance.InventoryInitValue == false)
            {
                NetworkInventoryInfoSaver.Instance.InventoryInitValue = true;
                DNFSceneManager.instance.UnLoadScene((int)SceneIndex.Inventory);
            }
        }
	}

    public void Load()
    {
        if (_itemSaveManager != null)
        {
            _itemSaveManager.LoadEquipment();
            _itemSaveManager.LoadInventory();
            _chracterStatInfo.SetCharacterInfo();
        }
    }

    public void LoadMulti()
    {
        if (_itemSaveManager != null)
        {
            _itemSaveManager.LoadItemsByNetID();
            _chracterStatInfo.SetCharacterInfo();
        }
    }

    public void SaveSingle()
	{
		if (_itemSaveManager != null)
		{
			_itemSaveManager.SaveEquipment();
			_itemSaveManager.SaveInventory();
		}
	}

	public void SaveMulti()
	{
		NetworkInventoryInfoSaver.Instance.SaveItemIDs(_equipmentPanel.EquipmentSlots, _inventory.ItemSlots);
		NetworkManager.Instance.CloseInventory();
	}

	private void OnDestroy()
	{
		if (_itemSaveManager != null)
		{
			_itemSaveManager.SaveEquipment();
			_itemSaveManager.SaveInventory();
		}
	}


	private void EquipFromInventory(Item item)
	{
		if (item is EquipableItem)
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
		if (_inventory.RemoveItem(item))
		{
			EquipableItem previousItem;
			if (_equipmentPanel.AddItem(item, out previousItem))
			{
				if (previousItem != null)
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
		if (!_inventory.IsFull() && _equipmentPanel.RemonveItem(item))
		{
			_inventory.AddItem(item);
		}
	}
}
