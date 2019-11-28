using UnityEngine;
using System.Collections;

public class PotalTransport : Potal
{
    [SerializeField]
    private int _nextIndex;

    [SerializeField]
    private GameObject LightObject;
    [SerializeField]
    private GameObject BlockObject;

    public Transform[] spotGatePosition;
    
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

    protected override void Start()
    {
        base.Start();
        PotalBlock();
    }

    private void Update()
    {
        if(_isPotalBlock)
        {
            PotalBlock();
        }
        else
        {
            PotalUnBlock();
        }
    }

    private void PotalBlock()
    {
        BlockObject.SetActive(true);
        LightObject.SetActive(false);
    }
    private void PotalUnBlock()
    {
        BlockObject.SetActive(false);
        LightObject.SetActive(true);
    }

    public override void Enter()
    {
        base.Enter();
        MapLoader.instacne.ChangeRoom(_nextIndex, arrow);
    }
    public override Vector3 GetPlayerSpotPosition()
    {
        const int myPartyNumber = 0;
        // ToDo. Party 플레이시에 변경.
        return spotGatePosition[myPartyNumber].transform.position;
    }
    public override void SetActiveTransport(bool active)
    {
        _collider2D.enabled = active;
    }

}
