using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct MiniMapTile
{
    public Image image;
    public MiniMapTileStyle style;
    public Vector2 position;
}

public class MiniMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tile;
    private float _tileSize;

    [SerializeField]
    private GameObject _playerCursor;
    private Vector2 _playerCurrentPosition;

    private DungeonJsonData dungeonData;

    private List<MiniMapTile> _miniMapTiles;
    private int _witdh = 0;
    private int _height = 0;

    [SerializeField]
    private Transform _tilesTransform;

    // Use this for initialization
    void Start()
    {
        _miniMapTiles = new List<MiniMapTile>();
        Initialized();
    }

    private void Initialized()
    {
        dungeonData = MapLoader.instacne.dungeonData;

        _tileSize = _tile.GetComponent<Image>().rectTransform.rect.width;

        _playerCurrentPosition = dungeonData.DungeonInfos[0].position;

        foreach (var item in dungeonData.DungeonInfos)
        {
            MaxWidth((int)item.position.x);
            MaxHeight((int)item.position.y);

            MiniMapTile mapTile;
            mapTile.style = GetTileStyle(item);

            mapTile.position = item.position;
 
            mapTile.image = Instantiate(_tile, _tilesTransform).GetComponent<Image>();
            _miniMapTiles.Add(mapTile);            
        }
        InitializedPlayerCursor();
        InitializedMiniTile();
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
    
    private MiniMapTileStyle GetTileStyle(DungeonInfo dungeonInfo)
    {
        int minimapStyle = 0;
        foreach (var potal in dungeonInfo.potalTransportinfos)
        {
            minimapStyle += (int)ConvertMiniMapArrow(potal.arrow);
        }
        return (MiniMapTileStyle)minimapStyle;
    }
    private void InitializedPlayerCursor()
    {
        _playerCursor.transform.localPosition
                = new Vector3((_playerCurrentPosition.x - _witdh) * _tileSize,
                (_playerCurrentPosition.y - _height) * _tileSize, 0);
    }
    private void InitializedMiniTile()
    {
        foreach (var item in _miniMapTiles)
        {
            item.image.transform.localPosition
                = new Vector3((item.position.x - _witdh) * _tileSize,
                (item.position.y - _height) * _tileSize, 0);
            UIHelper.Instance.miniMap.ChangeTileImage(item);
        }
    }

    private MiniMapTileStyle ConvertMiniMapArrow(ARROW arrow)
    {
        switch(arrow)
        {
            case ARROW.UP:
                {
                    return MiniMapTileStyle.Up;
                }
            case ARROW.DOWN:
                {
                    return MiniMapTileStyle.Down;
                }
            case ARROW.LEFT:
                {
                    return MiniMapTileStyle.Left;
                }
            case ARROW.RIGHT:
                {
                    return MiniMapTileStyle.Right;
                }
        }
        return MiniMapTileStyle.Unknown;
    }
}
