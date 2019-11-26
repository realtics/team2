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

    //void FixedUpdate()
    //{
    //    if (FindActiveCurrentPotal())
    //    {
    //        _currentPotal.Enter();
    //        ResetPotals();
    //        FIndPotals();
    //    }
    //}

    public void PotalEnter()
    {
        ResetPotals();
    }

    private void FIndPotals()
    {
        _potals = FindObjectsOfType<Potal>();
    }
    private bool FindActiveCurrentPotal()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            if (_potals[i].IsPlayerEnter)
            {
                _currentPotal = _potals[i];
                return true;
            }
        }
        return false;
    }

    private void ResetPotals()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            _potals[i].Reset();
        }
    }
}
