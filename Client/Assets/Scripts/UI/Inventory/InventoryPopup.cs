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

    private void Update()
    {
        if (NetworkInventoryInfoSaver.Instance.RES_INVENTORY_OPEN)
        {
            DNFSceneManager.Instance.LoadSceneAddtive((int)SceneIndex.Inventory);
            NetworkInventoryInfoSaver.Instance.RES_INVENTORY_OPEN = false;
        }
    }

    public void ClickInventory()
	{
        if(NetworkManager.Instance.IsSingle)
		DNFSceneManager.Instance.LoadSceneAddtive((int)SceneIndex.Inventory);

        else
        {
            NetworkManager.Instance.OpenInventory();
        }
	}
}
