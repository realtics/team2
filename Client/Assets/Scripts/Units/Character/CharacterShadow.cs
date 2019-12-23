using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShadow : MonoBehaviour
{
    public Transform shadowImage;
    public Transform avatarTransform;

    void Start()
    {
        
    }

    void Update()
    {
        SetShadowDeltaSize();
    }

    private void SetShadowDeltaSize()
    {
        float height = avatarTransform.localPosition.y;

        if (avatarTransform.localPosition.y <= 0.0f)
            return;

        Vector3 shadowScale = Vector3.one;

        float scale = Mathf.Clamp(1.0f - height * 0.15f, 0.5f, 1.0f);

        shadowScale.x = scale;
        shadowScale.y = scale;

        shadowImage.localScale = shadowScale;
    }
}
