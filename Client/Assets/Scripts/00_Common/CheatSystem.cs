using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSystem : MonoBehaviour
{
    public GameObject InputSystem;
    public GameObject VirtualPad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            InputSystem.gameObject.SetActive(true);
            VirtualPad.gameObject.SetActive(false);
            FindObjectOfType<InputSystem>().FindPlayerCharacter();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            InputSystem.gameObject.SetActive(false);
            VirtualPad.gameObject.SetActive(true);
            FindObjectOfType<MovementJoystick>().FindPlayerCharacter();
        }
    }
}
