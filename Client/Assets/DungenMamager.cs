using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungenMamager : MonoBehaviour
{
    [SerializeField]
    private Potal[] _potals;
    private Potal _currentPotal;
    private ARROW _arrow;

    private JsonManagement _jsonManagement;

    // Start is called before the first frame update
    void Start()
    {
        FIndPotals();
        _jsonManagement = new JsonManagement();
    }

    // Update is called once per frame
    void Update()
    {
        if(SetActiveCurrentPotal())
        {
            DestroyAllFieldObject();

            LoadPotalDungeon();

            TeardownPotals();
        }
    }
    public void SaveDungen()
    {
        // Test.. 
        _jsonManagement.JsonSave();
        
    }
    private void LoadPotalDungeon()
    {
        JsonData data = _jsonManagement.JsonLoad<JsonData>(_currentPotal.crossDungenName);
        GameObject obj = (GameObject)Resources.Load(data.filePath);
        obj.transform.position = data.position;
        Instantiate(obj);
    }

    private void FIndPotals()
    {
       _potals = FindObjectsOfType<Potal>();
    }
    private bool SetActiveCurrentPotal()
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

    private void TeardownPotals()
    {
        for (int i = 0; i < _potals.Length; i++)
        {
            _potals[i].Teardown();
        }
    }
    private void DestroyAllFieldObject()
    {
        
    }
}
