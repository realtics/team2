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
		if (NetworkManager.Instance.IsSingle)
		{
			InventoryManager.Instance.Save();
			DNFSceneManager.instance.UnLoadScene((int)SceneIndex.Inventory);
		}
		else
		{
			InventoryManager.Instance.SaveNetwork();
			//FIXME 패킷받앗을때 호출
			DNFSceneManager.instance.UnLoadScene((int)SceneIndex.Inventory);
		}
	}
}