using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungenMamager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _potals;
    private Potal _currentPotal;
    private ARROW _arrow;

    private JsonManagement _jsonManagement;

    // Start is called before the first frame update
    void Start()
    {
        _potals = GameObject.FindGameObjectsWithTag("Potal");
        _jsonManagement = new JsonManagement();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPotalsActiveAndCurrentPotal())
        {
            Debug.Log("Active");

            LoadPotalDungen();

            TeardownPotals();
        }

    }
    public void SaveDungen()
    {
        // Test.. 
        _jsonManagement.JsonSave();
        
    }
    private void LoadPotalDungen()
    {
        _jsonManagement.JsonLoad(_currentPotal.crossDungenName);
    }

    private bool IsPotalsActiveAndCurrentPotal()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            if (_potals[i].GetComponent<Potal>().IsPlayerEnter)
            {
                _currentPotal = _potals[i].GetComponent<Potal>();

                return true;
            }
        }
        return false;
    }
    private void TeardownPotals()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            _potals[i].GetComponent<Potal>().Teardown();
        }
    }

}
