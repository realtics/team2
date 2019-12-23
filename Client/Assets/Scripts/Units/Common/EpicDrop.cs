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

    public void SetHellItem(Item item)
    {
        _itemIcon.sprite = item.icon;
        _itemText.text = item.itemName;
        _itemTextField.transform.localScale = new Vector3(item.itemName.Length/2, 1, 1);
    }
}
