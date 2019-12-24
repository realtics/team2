using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu]
public class ResultItemDatabase : ItemDatabase
{
#if UNITY_EDITOR
	protected override void LoadItems()
	{
		items = FindAssetsByType<Item>("Assets/ScriptableObjects/Items/resultItems");
	}
#endif
}
