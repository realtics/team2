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

    private CharacterMovement _player;
    public CharacterMovement Player
    {
        get
        {
            return _player;
        }

    }

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<CharacterMovement>().GetComponent<CharacterMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
