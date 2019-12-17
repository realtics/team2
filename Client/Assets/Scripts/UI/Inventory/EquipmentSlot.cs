
public class EquipmentSlot : ItemSlot
{
    public EquipmentType equipmentType;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = equipmentType.ToString() + " Slot";
    }

	protected override void FindToolTip()
	{
		if (_itemToolTip == null)
			_itemToolTip = InventoryManager.Instance.EquipPanelTooltip;
	}

}
