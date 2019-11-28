using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum DungenClearMenu
{
    Retry,
    OtherDugeon,
    Retrun
}

public class UIDungeonClearMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] _menuButton;

    // Start is called before the first frame update
    void Start()
    {
        //_menuButton[(int)DungenClearMenu.Retry].onClick.AddListener(() => { DungeonGameManager.Instance.MoveToScene((int)SceneIndex.Dungen); });
        //_menuButton[(int)DungenClearMenu.OtherDugeon].onClick.AddListener(() => { DungeonGameManager.Instance.MoveToScene((int)SceneIndex.Dungen); });
        _menuButton[(int)DungenClearMenu.Retrun].onClick.AddListener(() => { DungeonGameManager.Instance.MoveToScene((int)SceneIndex.Lobby); });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
