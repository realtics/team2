using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; 
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
 
    }

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
