using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
   [SerializeField]
    private float _deleteTime;
    private float _currentTime;

    private void Awake()
    {
        _currentTime = 0.0f;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _deleteTime && gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
            _currentTime = 0.0f;
        }
    }
}
