using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IPAddressInputField : MonoBehaviour
{
    public InputField _ipFeild;

    public void OnValueChanged()
    {
        NetworkManager.Instance.SetIPAddress(_ipFeild.text);
    }
}
