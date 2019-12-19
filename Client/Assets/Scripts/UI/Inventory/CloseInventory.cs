using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseInventory : MonoBehaviour
{
	private Button _button;

	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(ClickReturn);
	}

	public void ClickReturn()
	{
		InventoryManager.Instance.Save();
		DNFSceneManager.instance.UnLoadScene((int)SceneIndex.Inventory);
	}
}