using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
	private static EffectManager _instance;
	public static EffectManager Instance
	{
		get
		{
			return _instance;
		}
	}

	[SerializeField]
	private GameObject _effectPrefab;
	[SerializeField]
	private GameObject _hitDamagePrefab;
	[SerializeField]
	private GameObject _clearCircle;

	private void Awake()
	{
		_instance = this;
	}

	// Start is called before the first frame update
	private void Start()
    {
		ObjectPoolManager.Instance.CreatePool(_effectPrefab, 5);
		ObjectPoolManager.Instance.CreatePool(_hitDamagePrefab, 5);
	}

	public void AddHitEffect(Vector3 position, float size)
	{
		GameObject hitEffect = ObjectPoolManager.Instance.GetRestObject(_effectPrefab);
		hitEffect.transform.position = position;
		hitEffect.transform.localScale = new Vector3(size, size, 0);
	}

	public void AddHitDamageEffect(Vector3 position, float damage)
	{
		GameObject hitDamage = ObjectPoolManager.Instance.GetRestObject(_hitDamagePrefab);
		hitDamage.transform.position = position;
		hitDamage.GetComponent<Damage>().SetDamage(damage);
	}

	public void SpawnClearCircle(Vector3 position)
	{
		GameObject clearCircle = ObjectPoolManager.Instance.GetRestObject(_clearCircle);
		clearCircle.transform.position = position;
	}
}
