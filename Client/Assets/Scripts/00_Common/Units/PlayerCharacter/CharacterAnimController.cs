using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    private GameObject _attackBox;

    private void Start()
    {
        _attackBox = transform.GetChild(0).gameObject;
        _attackBox.SetActive(false);
    }

    private void Update()
    {
    }

    public void OnAttackBox()
    {
        _attackBox.SetActive(true);
    }

    public void OffAttackBox()
    {
        _attackBox.SetActive(false);
    }
}
