//using System.Collections;
//using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

//[CreateAssetMenu]
//public class ItemDatabase : ScriptableObject
//{
//	[SerializeField]
//	private Item[] items;

//	public Item GetItemReference(string itemID)
//	{
//		foreach (Item item in items)
//		{
//			if(item.ID == itemID)
//			{
//				return item;
//			}
//		}
//		return null;
//	}

//	public Item GetItemCopy(string itemID)
//	{
//		Item item = GetItemReference(itemID);
//		return item != null ? item.GetCopy() : null;
//	}

//#if UNITY_EDITOR
//	private void OnValidate()
//	{
//		LoadItems();
//	}
//}
