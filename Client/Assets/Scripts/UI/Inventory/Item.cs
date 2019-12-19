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

	public virtual Item GetCopy()
	{
		return this;
	}

	public virtual void Destroy()
	{

	}

	public virtual string GetItemType()
	{
		return "";
	}

	public virtual string GetDescription()
	{
		return "";
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		string path = AssetDatabase.GetAssetPath(this);
		_id = AssetDatabase.AssetPathToGUID(path);
	}
#endif
}

