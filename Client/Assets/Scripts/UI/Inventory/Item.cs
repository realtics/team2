using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu]
public class Item : ScriptableObject
{
	[SerializeField]
	private string _id;
	
    public string itemName;
    public Sprite icon;

	public string ID { get { return _id; } }

#if UNITY_EDITOR
	private void OnValidate()
	{
		string path = AssetDatabase.GetAssetPath(this);
		_id = AssetDatabase.AssetPathToGUID(path);
	}
#endif
}

