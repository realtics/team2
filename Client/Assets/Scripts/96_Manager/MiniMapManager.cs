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
    private static MiniMapManager _instance;
    public static MiniMapManager instance
    { 
        get
        {
            return _instance;
        }
    
    }

    [SerializeField]
    private GameObject _tile;
    private float _tileSize;

    [SerializeField]
    private GameObject _playerCursor;
    [SerializeField]
    private GameObject _bossCursor;

    private DungeonJsonData dungeonData;

    private List<MiniMapTile> _miniMapTiles;
    private int _witdh = 0;
    private int _height = 0;

    [SerializeField]
    private Transform _tilesTransform;

    // Use this for initialization
    void Start()
    {
        _instance = this;
        _miniMapTiles = new List<MiniMapTile>();
        Initialized();
    }

    private void Initialized()
    {
        dungeonData = MapLoader.Instance.dungeonData;

        _tileSize = _tile.GetComponent<Image>().rectTransform.rect.width;

        foreach (var item in dungeonData.DungeonInfos)
        {
            MaxWidth((int)item.position.x);
            MaxHeight((int)item.position.y);

            MiniMapTile mapTile;
            mapTile.style = GetTileStyle(item);

            mapTile.position = item.position;
 
            mapTile.image = Instantiate(_tile, _tilesTransform).GetComponent<Image>();
            _miniMapTiles.Add(mapTile);          
            
            if(item.isBoss)
            {
                InitializedBossCursor(item.position);
            }
        }

        movePlayerCursor(dungeonData.DungeonInfos[0].position);

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

    public void movePlayerCursor(Vector2 playerPosition)
    {
        _playerCursor.transform.localPosition
                = new Vector3((playerPosition.x - _witdh) * _tileSize,
                (playerPosition.y - _height) * _tileSize, 0);
    }
    public void InitializedBossCursor(Vector2 BossPosition)
    {
        _bossCursor.transform.localPosition
                = new Vector3((BossPosition.x - _witdh) * _tileSize,
                (BossPosition.y - _height) * _tileSize, 0);
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
