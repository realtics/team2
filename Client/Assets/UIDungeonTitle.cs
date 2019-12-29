using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonTitle : MonoBehaviour
{
    [SerializeField]
    private Text _title;

    public void SetTitle(string name)
    {
        _title.text = name;
    }
}
