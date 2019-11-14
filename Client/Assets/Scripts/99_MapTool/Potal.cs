using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ARROW
{
    UP = 0,
    DOWN,
    LEFT,
    RIGHT
}

public class Potal : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public ARROW arrow;
    public string crossDungenName;

    private bool _isPlayerEnter;
    public bool IsPlayerEnter { get { return _isPlayerEnter; } }

    bool isPotalBlock;

    public Potal()
    {
        isPotalBlock = false;
        _isPlayerEnter = false;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPotalBlock)
        {
            PlayerEnterPotal();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPotalBlock = false;
    }
    private void PlayerEnterPotal()
    {
        _isPlayerEnter = true;
    }
    
    public void Teardown()
    {
        isPotalBlock = false;
        _isPlayerEnter = false;
    }
}
