using UnityEngine;
using System.Collections;

public class Layer : MonoBehaviour
{
    public int id;
    public int priority;

    public Layer(int layerId, int layerPriority)
    {
        id = layerId;
        priority = layerPriority;
    }
}
