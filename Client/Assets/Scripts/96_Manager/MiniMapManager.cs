using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct MiniMapTile
{
    public Image image;
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
            MaxWidth((int)item.position.x);
            MaxHeight((int)item.position.y);

            int minimapArrow = 0;

            foreach(var potal in item.potalTransportinfos)
            {
                minimapArrow += (int)ConvertMiniMapArrow(potal.arrow);
            }

            MiniMapTile mapTile;
            mapTile.arrow = (MiniMapArrow)minimapArrow;

            mapTile.position = item.position;
 
            mapTile.image = Instantiate(_tilePrefab, parent).GetComponent<Image>();
            _miniMapTiles.Add(mapTile);            
        }

        foreach (var item in _miniMapTiles)
        {
            item.image.transform.localPosition
                = new Vector3((item.position.x - _witdh) * _tileSize,
                (item.position.y - _height) * _tileSize, 0);
            UIHelper.Instance.miniMap.ChangeTileImage(item);
        }
    }
    private void MaxWidth(int x)
    {
        if (_witdh < x)
            _witdh = x;
    }
    private void MaxHeight(int y)
    {
        if (_height < y)
            _height = y;
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
