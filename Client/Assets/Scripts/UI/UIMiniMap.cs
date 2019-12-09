﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum MiniMapArrow
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

    //Right = 0,
    //Up,
    //UpRight,
    //Left,
    //LeftRight,
    //UpLeft,
    //UpLeftRight,
    //Down,
    //DownRight,
    //UpDown,
    //UpDownRight,
    //DownLeft,
    //DownLeftRight,
    //UpDownLeft,
    //UpDownLeftRight,
    //Unknown
}



public class UIMiniMap : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _tiles;
    [SerializeField]
    private Sprite[] _blinkTiles;

    // Use this for initialization
    void Start()
    {

    }

    public void ChangeTileImage(MiniMapTile mapTile)
    {
        mapTile.image.sprite = _tiles[(int)mapTile.arrow];
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
