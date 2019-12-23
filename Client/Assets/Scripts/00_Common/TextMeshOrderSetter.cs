using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshOrderSetter : MonoBehaviour
{
    public BaseUnit unit;
	public TextMeshPro textMesh;

    private float _forward;
    private float _initScaleX;

    void Start()
    {
        _initScaleX = transform.localScale.x;
        FlipImage();
    }

    // Update is called once per frame
    void Update()
    {
		textMesh.sortingOrder = -(int)(transform.parent.position.y * 10.0f);
        FlipImage();
    }

    private void FlipImage()
    {
        if (_forward == unit.Forward)
            return;

        Vector3 scale = transform.localScale;
        scale.x = _initScaleX * unit.Forward;
        transform.localScale = scale;

        _forward = unit.Forward;

    }

}
