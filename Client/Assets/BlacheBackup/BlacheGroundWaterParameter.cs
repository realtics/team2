using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacheGroundWaterParameter : MonoBehaviour
{
    [SerializeField]
    private List<Animator> _waters;

    private void Awake()
    {
    }

    private void ClipEvent_OffWater()
    {
        foreach (Animator anim in _waters)
        {
            anim.SetBool("End", true);
        }
    }
}
