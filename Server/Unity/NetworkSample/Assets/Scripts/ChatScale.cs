using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatScale : MonoBehaviour
{
    public void ResizeMinichatPanel()
    {
        Vector2 size = ((RectTransform)transform).sizeDelta;
        size.y = transform.childCount * 24;
        ((RectTransform)transform).sizeDelta = size;
    }
}
