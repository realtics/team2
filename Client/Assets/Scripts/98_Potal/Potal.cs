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
        if (!_isPotalBlock)
        {
            PlayerEnterPotal();
            // hack. 테스트용 끝나고 지워.
            collision.gameObject.transform.position = new Vector3(0, 0, 0);
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
    
    public void Reset()
    {
        _isPotalBlock = false;
        _isPlayerEnter = false;
    }
    public virtual void Enter()
    {

    }

}
