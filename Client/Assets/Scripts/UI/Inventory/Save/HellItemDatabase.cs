using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu]
public class HellItemDatabase : ItemDatabase
{
#if UNITY_EDITOR
	protected override void LoadItems()
	{
		items = FindAssetsByType<Item>("Assets/ScriptableObjects/Items/hellItems");
	}
#endif
}
