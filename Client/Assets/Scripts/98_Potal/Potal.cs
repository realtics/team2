using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ARROW
{
    UP = 0,
    DOWN,
    LEFT,
    RIGHT,
    NULL
}

public class Potal : MonoBehaviour
{
    public ARROW arrow;

    protected bool _isPotalBlock;
    private bool _isFirstTranportPotalBlock;

    protected BoxCollider2D _collider2D;

    private void Start()
    {
        _isPotalBlock = false;
        _isFirstTranportPotalBlock = false;
        _collider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!_isPotalBlock && !_isFirstTranportPotalBlock)
            {
                PlayerEnterPotal();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isFirstTranportPotalBlock = false;
    }
    private void PlayerEnterPotal()
    {
        Enter();
    }

    public void Block()
    {
        _isPotalBlock = true;
    }
    public void FirstTranportPotalBlock()
    {
        _isFirstTranportPotalBlock = true;
    }

    public bool IsArrowPotal(ARROW potalArrow)
    {
        if (arrow == potalArrow)
            return true;
        else
            return false;
    }

    public virtual Vector3 GetPlayerSpotPosition()
    {
        return Vector3.zero;
    }

    public void Reset()
    {
        _isPotalBlock = false;
        _isFirstTranportPotalBlock = false;
    }
    public virtual void SetActiveTransport(bool active)
    {

    }

    public virtual void Enter()
    {
        PotalManager.instance.PotalEnter();
    }
}
