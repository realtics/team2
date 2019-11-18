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
    private SpriteRenderer _spriteRenderer;

    public ARROW arrow;
    public string crossDungenName;

    private bool _isPlayerEnter;
    public bool IsPlayerEnter { get { return _isPlayerEnter; } }

    private bool _isPotalBlock;

    public Potal()
    {
        _isPotalBlock = false;
        _isPlayerEnter = false;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isPotalBlock)
        {
            PlayerEnterPotal();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isPotalBlock = false;
    }
    private void PlayerEnterPotal()
    {
        _isPlayerEnter = true;
    }
    
    public void Teardown()
    {
        _isPotalBlock = false;
        _isPlayerEnter = false;
    }
}
