using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PotalManager : MonoBehaviour
{

    private static PotalManager _instance;
    public static PotalManager instance
    {
        get
        {
            return _instance;
        }
    }
  
    [SerializeField]
    private Potal[] _potals;
    private Potal _currentPotal;
    private ARROW _arrow;

    void Start()
    {
        _instance = this;
        FIndPotals();
    }

    public void PotalEnter()
    {
        ResetPotals();
    }

    public void FIndPotals()
    {
        _potals = FindObjectsOfType<Potal>();
    }

    public Vector3 FindGetArrowPotalPosition(ARROW arrow)
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            if (_potals[i].IsArrowPotal(arrow))
            {
                return _potals[i].GetPlayerSpotPosition();
            }
        }
        return Vector3.zero;
    }

    public void BlockPotals()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            _potals[i].Block();
        }
    }
    public void ResetPotals()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            _potals[i].Reset();
        }
    }
}
