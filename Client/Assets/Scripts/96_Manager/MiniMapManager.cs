using UnityEngine;
using System.Collections;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab;

    private int maxNum;
    private DungeonJsonData dungeonData;

    // Use this for initialization
    void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        dungeonData = MapLoader.instacne.dungeonData;
        maxNum = dungeonData.DungeonInfos.Length;

        Transform parent = UIHelper.Instance.miniMap.gameObject.transform;

        Instantiate(_tilePrefab, parent);
    }

}
