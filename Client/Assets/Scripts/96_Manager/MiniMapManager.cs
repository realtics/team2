using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct MiniMapTile
{
    public GameObject tile;
    public MiniMapArrow arrow;
    public Vector2 position;
}

public class MiniMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab;
    private float _tileSize;

    private int maxNum;
    private DungeonJsonData dungeonData;

    private List<MiniMapTile> _miniMapTiles;
    private int _witdh = 0;
    private int _height = 0;

    // Use this for initialization
    void Start()
    {
        _miniMapTiles = new List<MiniMapTile>();
        Initialized();
    }

    private void Initialized()
    {
        dungeonData = MapLoader.instacne.dungeonData;
        maxNum = dungeonData.DungeonInfos.Length;
        Transform parent = UIHelper.Instance.miniMap.gameObject.transform;

        _tileSize = _tilePrefab.GetComponent<Image>().rectTransform.rect.width;

        
        // 던전 하나의 방향 정보 얻기.
        foreach(var item in dungeonData.DungeonInfos)
        {
            _witdh += (int)item.position.x;
            _height += (int)item.position.y;

            int minimapArrow = 0;

            foreach(var potal in item.potalTransportinfos)
            {
                minimapArrow += (int)ConvertMiniMapArrow(potal.arrow);
            }

            MiniMapTile mapTile;
            mapTile.arrow = (MiniMapArrow)minimapArrow;

            mapTile.position = item.position;
 
            mapTile.tile = null;
            _miniMapTiles.Add(mapTile);

            GameObject @object = Instantiate(_tilePrefab, parent);
            @object.transform.localPosition = new Vector3(-(mapTile.position.x * _tileSize), mapTile.position.y * _tileSize, 0);
        }



        
    }

    private MiniMapArrow ConvertMiniMapArrow(ARROW arrow)
    {
        switch(arrow)
        {
            case ARROW.UP:
                {
                    return MiniMapArrow.Up;
                }
            case ARROW.DOWN:
                {
                    return MiniMapArrow.Down;
                }
            case ARROW.LEFT:
                {
                    return MiniMapArrow.Left;
                }
            case ARROW.RIGHT:
                {
                    return MiniMapArrow.Right;
                }
        }
        return MiniMapArrow.Unknown;
    }
}
