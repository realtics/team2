using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPopup : MonoBehaviour
{
	private Button _button;

	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(ClickInventory);
	}

	public void ClickInventory()
	{
		DNFSceneManager.instance.LoadSceneAddtive((int)SceneIndex.Inventory);

	}
}
