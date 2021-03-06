﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
	[SerializeField] protected Item[] items;

	public string GetRandomItemID(out Sprite icon, out string itemName)
	{
		int startNum = 0;
		int endNum = items.Length;
		int randNum = Random.Range(startNum, endNum);

		icon = items[randNum].icon;
		itemName = items[randNum].itemName;
		return items[randNum].ID;
	}

    public string GetRandomItemID()
    {
        int startNum = 0;
        int endNum = items.Length;
        int randNum = Random.Range(startNum, endNum);

        return items[randNum].ID;
    }

    public Item GetItemReference(string itemID)
	{
		foreach (Item item in items)
		{
			if (item.ID == itemID)
			{
				return item;
			}
		}
		return null;
	}

	public Item GetItemCopy(string itemID)
	{
		Item item = GetItemReference(itemID);
		return item != null ? item.GetCopy() : null;
	}

	public Item GetItemReferenceByNetId(string itemID)
	{
		foreach (Item item in items)
		{
			if (item.NetID == itemID)
			{
				return item;
			}
		}
		return null;
	}

	public Item GetItemCopyByNetId(string itemID)
	{
		Item item = GetItemReferenceByNetId(itemID);
		return item != null ? item.GetCopy() : null;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		LoadItems();
	}

	private void OnEnable()
	{
		EditorApplication.projectChanged -= LoadItems;
		EditorApplication.projectChanged += LoadItems;
	}

	private void OnDisable()
	{
		EditorApplication.projectChanged -= LoadItems;
	}

	protected virtual void LoadItems()
	{
		items = FindAssetsByType<Item>("Assets/ScriptableObjects/Items");
	}

	//Slightly modified version of this answer: http://answers.unity.com/answers/1216386/view.html
	public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
	{
		string type = typeof(T).Name;

		string[] guids;
		if (folders == null || folders.Length == 0)
		{
			guids = AssetDatabase.FindAssets("t:" + type);
		}
		else
		{
			guids = AssetDatabase.FindAssets("t:" + type, folders);
		}

		T[] assets = new T[guids.Length];

		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
			assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
		}
		return assets;
	}
#endif
}
