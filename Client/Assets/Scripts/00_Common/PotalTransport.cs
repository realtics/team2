using UnityEngine;
using System.Collections;

public class PotalTransport : Potal
{
    [SerializeField]
    private int _nextIndex;

    public int nextIndex
    { 
        get
        {
            return _nextIndex;
        }
        set
        {
            _nextIndex = value;
        }
    }

    public override void Enter()
    {
        MapLoader.instacne.ChangeDungeon(_nextIndex);
    }
}
