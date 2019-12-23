using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshOrderSetter : MonoBehaviour
{
	public TextMeshPro textMesh;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		textMesh.sortingOrder = -(int)(transform.parent.position.y * 10.0f);
	}

}
