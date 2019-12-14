using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private ItemToolTip _itemToolTip;

    public event Action<Item> OnRightClickEvent;

    private Item _item;
    public Item Item
    { get { return _item; }
      set
        {
            _item = value;

            if (_item == null)
                _image.enabled = false;
            else
            {
                _image.sprite = _item.icon;
                _image.enabled = true;
            }
        } 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if(eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
                OnRightClickEvent(Item);
        }
    }

    protected virtual void OnValidate()
    {
        if (_image == null)
            _image = GetComponent<Image>();

        //게임실행중이면 느릴거지만, 어차피 에디터에서 찾고 게임중엔 찾지않을 것이 분명
        if (_itemToolTip == null)
            _itemToolTip = FindObjectOfType<ItemToolTip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Item is EquipableItem)
        {
            _itemToolTip.ShowToolTip((EquipableItem)Item);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _itemToolTip.HideToolTip();
    }
}
