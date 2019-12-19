using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteChromaKey : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public float threshold = 0.8f;

    [Range(0, 1)]
    public float slope = 0.2f;


    private SpriteRenderer _spriteRenderer;

    void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateChromakey(true);
    }

    void OnDisable()
    {
        UpdateChromakey(false);
    }

    void Update()
    {
        UpdateChromakey(true);
    }

    void UpdateChromakey(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_keyingColor", color);
        mpb.SetFloat("_thresh", threshold);
        mpb.SetFloat("_slope", slope);
        _spriteRenderer.SetPropertyBlock(mpb);
    }
}
