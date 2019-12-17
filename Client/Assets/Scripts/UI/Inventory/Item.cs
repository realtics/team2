using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class Item : ScriptableObject
{
	[SerializeField]
	private string _id;
	
    public string itemName;
    public Sprite icon;

	public string ID { get { return _id; } }

	private void OnValidate()
	{
		string path = AssetDatabase.GetAssetPath(this);
		_id = AssetDatabase.AssetPathToGUID(path);
	}
}
