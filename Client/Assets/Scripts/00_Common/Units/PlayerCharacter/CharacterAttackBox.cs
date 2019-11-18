using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackBox : MonoBehaviour
{
    private BaseUnit _unit;
    void Start()
    {
        _unit = transform.root.GetComponent<BaseUnit>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Contains("Monster"))
            return;

        if (Mathf.Abs(_unit.transform.position.y - other.transform.position.y) > 0.9f)
            return;

        other.transform.root.GetComponent<Monster>().OnHit(10.0f);

    }
}
