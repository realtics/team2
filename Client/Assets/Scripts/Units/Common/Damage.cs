using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour
{
	[SerializeField]
	private Transform _hitDamageTextObj;
	private TextMeshPro _hitDamageText;

	// Start is called before the first frame update
	private void Awake()
    {
		_hitDamageText = _hitDamageTextObj.GetComponent<TextMeshPro>();
	}

	public void SetDamage(float damage)
	{
		Debug.Log(damage);
		_hitDamageText.SetText(damage.ToString());
	}
}
