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

    }

    private void Initialized()
    {
        dungeonData = MapLoader.instacne.dungeonData;
        maxNum = dungeonData.DungeonInfos.Length;


    }

}
