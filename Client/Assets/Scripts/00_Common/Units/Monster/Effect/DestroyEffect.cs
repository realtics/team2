using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float deleteTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deleteTime -= Time.deltaTime;
        if (deleteTime <= 0)
            Destroy(this.gameObject);
    }
}
