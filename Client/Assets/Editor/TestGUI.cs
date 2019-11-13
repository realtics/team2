using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class TestGUI : MonoBehaviour
{
    public Vector2 position;
    public int layer;
    public Vector2 offset;


    private void Update()
    {
        if (Application.isPlaying)
        {
            Destroy(this);
        }
        else if(Selection.activeGameObject == this.gameObject)
        {
            position = offset * (Vector2)transform.position;
        }
    }
}