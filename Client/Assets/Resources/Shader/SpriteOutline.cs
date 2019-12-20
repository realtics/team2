using UnityEngine;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;
	public Color secondColor = Color.white;
	private Color32 lerpedColor;
	[Range(0, 25)]
	public int outlineSize = 1;

	private const float _changeTime = 1f;
	private float _currentTime = 0.0f;
	private bool _isChangeColor = false;

    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    private void OnDisable()
    {
        UpdateOutline(false);
    }

    private void FixedUpdate()
    {
		ChanageColor();
		UpdateOutline(true);
    }

    private void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);

		if (_isChangeColor)
		{
			lerpedColor = Color32.Lerp(color, secondColor, _currentTime);
			mpb.SetColor("_OutlineColor", lerpedColor);
		}
		else
		{
			lerpedColor = Color32.Lerp(secondColor, color, _currentTime);
			mpb.SetColor("_OutlineColor", lerpedColor);
		}

        mpb.SetFloat("_OutlineSize", outlineSize);
        _spriteRenderer.SetPropertyBlock(mpb);
    }

	private void ChanageColor()
	{
		_currentTime += Time.deltaTime*2;
		if (_currentTime >= _changeTime)
		{
			if (_isChangeColor)
				_isChangeColor = false;
			else
				_isChangeColor = true;

			_currentTime = 0.0f;
			lerpedColor = Color.black;
		}
	}
}

