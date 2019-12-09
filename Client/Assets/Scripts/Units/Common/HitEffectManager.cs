using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitEffectManager : MonoBehaviour
{
	private static HitEffectManager _instance;
	public static HitEffectManager Instance
	{
		get
		{
			return _instance;
		}
	}

	[SerializeField]
	private GameObject _effectPrefab;
	[SerializeField]
	private GameObject _HidDamagePrefab;
	private void Awake()
	{
		_instance = this;
	}

	// Start is called before the first frame update
	private void Start()
    {
		
	}

	public void AddHitEffect(Vector3 position, float size)
	{
		GameObject hitEffect = ObjectPoolManager.Instance.GetRestObject(_effectPrefab);
		hitEffect.transform.position = position;
		hitEffect.transform.localScale = new Vector3(size, size, 0);

		GameObject hitDamage = ObjectPoolManager.Instance.GetRestObject(_HidDamagePrefab);
		hitDamage.transform.position = position;
	}
}
