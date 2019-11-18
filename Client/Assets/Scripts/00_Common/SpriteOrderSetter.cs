using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderSetter : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.sortingOrder = -(int)(transform.parent.position.y * 10.0f);
    }
}
