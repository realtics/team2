using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIDungeonSelectMenu : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _dungeonSlot;
    [SerializeField]
    private Button _cancel;
    // Use this for initialization
    void Start()
    {
        foreach(var iter in _dungeonSlot)
        {
            _dungeonSlot[(int)DungenClearMenu.Retrun].GetComponent<Button>().onClick.AddListener(() => 
            { DungeonGameManager.Instance.MoveToScene((int)SceneIndex.Lobby); });
        }
        _cancel.onClick.AddListener(() => { gameObject.SetActive(false); });
    }
}
