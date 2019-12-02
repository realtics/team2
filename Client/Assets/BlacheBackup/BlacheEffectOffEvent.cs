using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacheEffectOffEvent : MonoBehaviour
{
    private Cinemachine.CinemachineImpulseSource impulseSource;

    // Start is called before the first frame update
    private void Awake()
    {
        impulseSource = gameObject.GetComponent<Cinemachine.CinemachineImpulseSource>();

    }
    void Start()
    {

    }
    private void OnEnable()
    {
        impulseSource.GenerateImpulse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClipEvent_OffActive()
    {
        gameObject.SetActive(false);
    }
}
