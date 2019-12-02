using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderSetterAtOrigin : MonoBehaviour
{
    private SpriteRenderer _renderer;
	private int _originLayer;
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
		_originLayer = _renderer.sortingOrder;

	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.sortingOrder = -(int)(transform.parent.position.y * 10.0f) + _originLayer;
    }
}
