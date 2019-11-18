using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public BoxCollider2D Map;
    private Transform target;

    private float xMax, xMin, yMax, yMin;

    // Start is called before the first frame update
    void Start()
    {
        target = player.transform;

        Vector3 min = Map.bounds.min;
        Vector3 max = Map.bounds.max;

        SetLimits(min, max);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float minClamp = Mathf.Clamp(target.position.x, xMin, xMax);
        float maxClamp = Mathf.Clamp(target.position.y, yMin, yMax);

        transform.position = new Vector3(minClamp, maxClamp, -10);
    }

    private void SetLimits(Vector3 min, Vector3 max)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = min.x + width / 2;
        xMax = max.x - width / 2;

        yMin = min.y + height / 2;
        yMax = max.y - height / 2;
    }
}
