using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum MiniMapTileStyle
{
    Unknown = 0,
    Right,
    Left,
    LeftRight,
    Down,
    DownRight,
    DownLeft,
    DownLeftRight,
    Up,
    UpRight,
    UpLeft,
    UpLeftRight,
    UpDown,
    UpDownRight,
    UpDownLeft,
    UpDownLeftRight
}



public class UIMiniMap : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _tiles;
    [SerializeField]
    private Sprite[] _blinkTiles;

    public void ChangeTileImage(MiniMapTile mapTile)
    {
        mapTile.image.sprite = _tiles[(int)mapTile.style];
    }

    public void Blinking(bool active)
    {
        if(active)
        {
            StartCoroutine(nameof(BlinkingTile));
        }
        else
        {
            StopCoroutine(nameof(BlinkingTile));
        }
    }
    public void Release()
    {
        StopCoroutine(nameof(BlinkingTile));


    }
    IEnumerable BlinkingTile()
    {

        // isActived 깜박깜박

        yield return new WaitForSeconds(1);
    }
}
