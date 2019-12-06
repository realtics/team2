using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MiniMapArrow
{
    Right = 0,
    Up,
    UpRight,
    Left,
    LeftRight,
    UpLeft,
    UpLeftRight,
    Down,
    DownRight,
    UpDown,
    UpDownRight,
    DownLeft,
    DownLeftRight,
    UpDownLeft,
    UpDownLeftRight,
    Unknown
}

public struct MiniMapTile
{
    public MiniMapArrow arrow;
    public Vector2 position;
}

public class UIMiniMap : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _tiles;
    [SerializeField]
    private Sprite[] _blinkTiles;

    private List<MiniMapTile> miniMapTiles;

    private const int _rectSize = 72;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
