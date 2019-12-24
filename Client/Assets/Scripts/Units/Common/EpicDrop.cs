using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EpicDrop : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _itemIcon;
	[SerializeField]
	private SpriteRenderer _itemTextField;
	[SerializeField]
	private TextMeshPro _itemText;

	private Item _item;

    public void SetHellItem(Item item)
    {
        _itemIcon.sprite = item.icon;
        _itemText.text = item.itemName;
        _itemTextField.transform.localScale = new Vector3(item.itemName.Length/2, 1, 1);

		_item = item;
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "UserPlayer")
		{
			gameObject.SetActive(false);
			ItemSaveIO.SaveResultItem(_item.ID);
		}
	}
}
