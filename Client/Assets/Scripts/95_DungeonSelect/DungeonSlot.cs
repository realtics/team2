using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DungeonSlot : MonoBehaviour
{
    [SerializeField]
    private string _dungeonFileName;
    public string dungeonFileName
    {
        get
        {
            return _dungeonFileName;
        }
    }
    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() =>
        { 
            MapLoader.Instance.SetMap(_dungeonFileName);
            DNFSceneManager.Instance.LoadScene((int)SceneIndex.Dungen);

			if (NetworkManager.Instance != null)
				NetworkManager.Instance.UserExit();
        });
    }
}
