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
    public ARROW arrow;
    public Vector3 transportPosition;

    private bool _isPlayerEnter;
    public bool IsPlayerEnter { get { return _isPlayerEnter; } }

    private bool _isPotalBlock;

    public Potal()
    {
        _isPotalBlock = false;
        _isPlayerEnter = false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

            if (!_isPotalBlock)
            {
                PlayerEnterPotal();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isPotalBlock = false;
    }
    private void PlayerEnterPotal()
    {
        Enter();
        _isPlayerEnter = true;
    }
    
    public void Reset()
    {
        _isPotalBlock = false;
        _isPlayerEnter = false;
    }
    public virtual void Enter()
    {
        PotalManager.instance.PotalEnter();
    }
}
